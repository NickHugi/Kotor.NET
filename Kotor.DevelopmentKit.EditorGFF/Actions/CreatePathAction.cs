using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

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
        var node = data.RootNode.NavigateTo<BaseFieldGFFNodeViewModel>(ExistingPath);
        data.FillPath(CreatePath);
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        var node = data.RootNode.NavigateTo<BaseFieldGFFNodeViewModel>(ExistingPath);
        var createdRootNode = data.RootNode.NavigateTo<BaseFieldGFFNodeViewModel>(CreatePath, new NodePath(ExistingPath.First() as string))!;
        createdRootNode.Delete();
    }
}
