using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class CreatePathAction : IAction<GFFResourceEditorViewModel>
{
    public NodePath ExistingPath { get; }
    public NodePath CreatePath { get; }

    public CreatePathAction(NodePath existingPath, NodePath createPath)
    {
        ExistingPath = existingPath;
        CreatePath = createPath;
    }

    public void Apply(GFFResourceEditorViewModel data)
    {
        var node = data.RootNode.NavigateTo<BaseFieldGFFNode>(ExistingPath);
        data.FillPath(CreatePath);
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        var node = data.RootNode.NavigateTo<BaseFieldGFFNode>(ExistingPath);
        var createdRootNode = data.RootNode.NavigateTo<BaseFieldGFFNode>(CreatePath, new NodePath(ExistingPath.First() as string))!;
        createdRootNode.Delete();
    }
}
