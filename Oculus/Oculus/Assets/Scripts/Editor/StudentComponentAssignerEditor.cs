using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;


[CustomEditor(typeof(StudentComponentAssigner))]
[CanEditMultipleObjects]
public class StudentComponentAssignerEditor : Editor
{
    public string[] Rigs = { "RightArm", "LeftArm", "RightLeg", "LeftLeg", "BodyMoving" };
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Node"))
        {
            var studentComp = (StudentComponentAssigner)target;
            CloneChildren(studentComp);
            SnapInteractionProperties(studentComp);
            TwoBoneIKProperties(studentComp);
            CloneParentComponents(studentComp);
            ActivateRigs(studentComp);
            SetupComponent(studentComp, studentComp.GetComponent<StudentController>());
            SetupComponent(studentComp, studentComp.GetComponentInChildren<KickBoardController>(true));
            SetupComponent(studentComp, studentComp.GetComponent<StudentExtension>());
            SetUpBodyMoving(studentComp);

        }
    }
    private void CloneChildren(StudentComponentAssigner studentComp)
    {
        if (studentComp.sourceGameObject == null) return;
        for (var i = 0; i < studentComp.sourceGameObject.transform.childCount; i++)
        {
            var child = studentComp.sourceGameObject.transform.GetChild(i);
            if (studentComp.transform.Find(child.name) == null)
            {
                var newChild = Instantiate(child, studentComp.transform);
                newChild.name = child.name;
            }
        }
    }
    private void CloneParentComponents(StudentComponentAssigner studentComp)
    {
        var allComponents = studentComp.sourceGameObject.GetComponents<MonoBehaviour>();
        foreach (var comp in allComponents)
        {
            var exist = studentComp.transform.GetComponent(comp.GetType());
            if (exist == null)
            {
                Utils.CopyComponentV2(comp, studentComp.transform.gameObject);
            }
        }
        // PhotonView photonView = studentComp.GetComponent<PhotonView>();
        // photonView.ObservedComponents.Add(studentComp.GetComponent<PhotonTransformView>());
        SetupComponent(studentComp, studentComp.GetComponent<PhotonView>());
    }
    private void SnapInteractionProperties(StudentComponentAssigner comp)
    {
        foreach (var rig in Rigs)
        {
            var rigTransform = comp.transform.Find(rig);
            if (rigTransform != null)
            {
                var allInteractions = rigTransform.GetComponentsInChildren<SnapInteraction>(true);
                foreach (var inter in allInteractions)
                {
                    if (inter.snapTo != null)
                    {
                        var transformName = inter.snapTo.name.ReplaceAll(" ", "");
                        var foundChild = comp.humanoidBody.FindChildRecursive(transformName, true);
                        if (foundChild != null)
                        {
                            inter.snapTo = foundChild;
                            inter.transform.SetPositionAndRotation(foundChild.position, foundChild.rotation);
                        }
                        else
                        {
                            // Utils.LogError(this, "Couldn't find", transformName, inter.transform.parent.name);
                        }

                    }

                }

            }
        }
    }
    private void TwoBoneIKProperties(StudentComponentAssigner comp)
    {
        foreach (var rig in Rigs)
        {
            var targetRig = comp.transform.Find(rig);
            if (targetRig != null)
            {
                var twoBoneIKs = targetRig.GetComponentsInChildren<TwoBoneIKConstraint>();
                foreach (var k in twoBoneIKs)
                {
                    var root = comp.humanoidBody.FindChildRecursive(k.data.root.name, true);
                    var mid = comp.humanoidBody.FindChildRecursive(k.data.mid.name, true);
                    var tip = comp.humanoidBody.FindChildRecursive(k.data.tip.name, true);
                    if (root != null)
                    {
                        k.data.root = root;
                    }
                    if (mid != null)
                    {
                        k.data.mid = mid;
                    }
                    if (tip != null)
                    {
                        k.data.tip = tip;
                    }
                }
            }
        }
    }
    private void ActivateRigs(StudentComponentAssigner comp)
    {
        var rigBuilder = comp.GetComponent<RigBuilder>();
        rigBuilder.layers.Clear();
        foreach (var rigTransform in Rigs)
        {
            var targetRig = comp.transform.Find(rigTransform);
            if (targetRig != null)
            {
                if (targetRig.TryGetComponent<Rig>(out var rig))
                {
                    rigBuilder.layers.Add(new RigLayer(rig));
                }
            }
        }
    }
    private void SetUpBodyMoving(StudentComponentAssigner studentComp)
    {
        const string attachPointName = "AttactCenterPoint";
        var attachBody = studentComp.transform.FindChildRecursive(attachPointName);
        var moving = studentComp.GetComponentInChildren<BodySnapInteraction>(true);
        if (attachBody == null)
        {
            var attachParent = studentComp.transform.FindChildRecursive("Bip001Spine2", true);
            var go = new GameObject(attachPointName);
            if (attachParent != null)
            {
                //go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                go.transform.parent = attachParent;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                moving.transform.SetPositionAndRotation(go.transform.position, go.transform.rotation);
            }
        }

        SetupComponent(studentComp, moving);
        var body = moving.GetType().GetField("wholeBody");
        body?.SetValue(moving, studentComp.transform);
    }
    private void SetupComponent(StudentComponentAssigner comp, Component target)
    {
        if (target == null)
        {
            Utils.LogError(this, "SetupComponent target is null");
            return;
        }

        var type = target.GetType();
        var fields = type.GetFields();
        // Utils.LogError(this, "SetupComponent ", type);
        foreach (FieldInfo field in fields)
        {
            var fieldType = field.FieldType;
            var fieldName = field.Name;
            if (fieldType == typeof(Transform))
            {
                var value = field.GetValue(target);
                if (value != null)
                {
                    var transformName = ((Transform)value).name;
                    Transform transform = null;
                    if (((Transform)value).name == comp.sourceGameObject.name) // root transform of target
                    {
                        transform = comp.transform;
                    }
                    else
                    {
                        transform = comp.transform.FindChildRecursive(transformName, true);
                    }

                    if (transform != null)
                    {
                        field.SetValue(target, transform);
                    }
                    else
                    {
                        Utils.LogError(this, "SetupComponent  Couldn't find transform[", transformName, "]inside compent", type, "Filed Name", fieldName);
                    }
                }
            }
            else
            {
                try
                {
                    var value = field.GetValue(target);
                    if (value != null)
                    {
                        if (fieldType.ToString().Contains("System.Collections.Generic.List"))
                        {
                            if (!fieldType.ToString().Contains("[UnityEngine.Material]"))
                            {
                                // Utils.LogError(this, "value", fieldName, value);
                                var list = new List<Component>((IEnumerable<Component>)value);
                                // Utils.LogError(this, "list", list);
                                var replacement = Utils.GetList(fieldType);
                                list.ForEach(component =>
                                {
                                    Transform foundComp = null;
                                    if (component.transform.name == comp.sourceGameObject.transform.name)
                                    {
                                        foundComp = comp.transform;
                                    }
                                    else
                                    {
                                        foundComp = comp.transform.FindChildRecursive(component.transform.name, true);
                                    }

                                    if (foundComp != null)
                                    {
                                        var cp = foundComp.GetComponent(component.GetType());
                                        if (cp != null)
                                        {
                                            replacement.Add(cp);
                                        }
                                        else
                                        {
                                            Utils.LogError(this, "Couldn't find comp", component.transform.name, component.GetType());
                                        }

                                    }
                                    else
                                    {
                                        Utils.LogError(this, "Couldn't find", component.transform.name);
                                    }

                                });
                                if (replacement.Count > 0)
                                {
                                    field.SetValue(target, replacement);

                                }
                            }

                        }
                        else
                        {
                            var checkComp = value is Component;
                            Component cp = null;
                            if (checkComp)
                            {
                                var myTrans = ((Component)value).transform;
                                var transName = myTrans.name;
                                var insideTrans = comp.transform.FindChildRecursive(transName, true);
                                if (insideTrans != null)
                                {
                                    cp = insideTrans.GetComponent(fieldType);
                                }
                                else
                                {
                                    cp = comp.gameObject.GetComponent(fieldType);
                                    if (cp == null)
                                    {
                                        var childrenCp = comp.gameObject.GetComponentsInChildren(fieldType, true);
                                        if (childrenCp != null && childrenCp.Length > 0)
                                        {
                                            cp = childrenCp[0];
                                        }
                                    }
                                }

                            }

                            if (cp != null)
                            {
                                field.SetValue(target, cp);
                            }
                        }

                    }

                }
                catch (Exception e)
                {
                    Utils.LogError(this, "Filed", fieldName, "Type", fieldType, "in Component", type, "Doesn't suport", e.ToString());
                }
            }

        }
    }
}
