using System;
using System.Collections.Generic;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.Data;
using Invert.Json;

public class ShellNodeConfig : ShellInheritableNode, IShellNodeTypeClass, IDocumentable
{
    public override bool AllowMultipleInputs
    {
        get { return true; }
    }

 
    private string _nodeLabel;
    private NodeColor _color;
    private bool _inheritable;
    private bool _isClass;

    public override void Document(IDocumentationBuilder docs)
    {
        docs.BeginSection(this.Name);
        docs.Section(this.Node.Name);
        docs.NodeImage(this);
        var className = FullName + "Node";
        var type = InvertApplication.FindType(className);
        if (type == null)
        {
            //Debug.Log("Couldn't find type in documentation " + className);
            // base.Document(docs);
        }
        else
        {
            var instance =  Activator.CreateInstance(type) as IDiagramNodeItem;
            if (instance == null)
            {
                //base.Document(docs);
            }
            else
            {
                instance.Document(docs);
            }

        }

        foreach (var item in PersistedItems.OfType<IShellNodeConfigItem>())
        {
            docs.BeginArea("NODEITEM");
            item.Document(docs);
            docs.EndArea();
        }
            
        docs.EndSection();
    }

    public string NodeLabel
    {
        get
        {
            if (string.IsNullOrEmpty(_nodeLabel))
                return Name;
            return _nodeLabel;
        }
        set { _nodeLabel = value; }
    }

    [InspectorProperty, JsonProperty]
    public NodeColor Color
    {
        get { return _color; }
        set
        {
        
            this.Changed("Color",ref _color, value);
        }
    }
    [InspectorProperty("Visual appearance of the node"), JsonProperty]
    public NodeStyle NodeStyle
    {
        get { return _nodeStyle; }
        set
        {
            this.Changed("NodeStyle", ref _nodeStyle, value);
        }
    }

    [InspectorProperty, JsonProperty]
    public bool Inheritable
    {
        get { return _inheritable; }
        set
        {
     
            this.Changed("Inheritable",ref _inheritable, value);
        }
    }

    [InspectorProperty, JsonProperty]
    public bool IsClass
    {
        get { return _isClass; }
        set
        {
    
            this.Changed("IsClass", ref _isClass, value);
        }
    }

    private SectionVisibility _visibility;
    private int _column;
    private int _row;
    private NodeStyle _nodeStyle;

    [InspectorProperty, JsonProperty]
    public int Row
    {
        get { return _row; }
        set
        {
            this.Changed("Row", ref _row, value);
        }
    }

    [InspectorProperty, JsonProperty]
    public int Column
    {
        get { return _column; }
        set
        {
     
            this.Changed("Column", ref _column, value);
        }
    }

    [InspectorProperty, JsonProperty]
    public SectionVisibility Visibility
    {
        get { return _visibility; }
        set
        {
            this.Changed("Visibility",ref _visibility, value);
        }
    }

    public string ReferenceClassName
    {
        get { return "I" + this.Name + "Connectable"; }
    }

    public override string ClassName
    {
        get { return this.Name + "Node"; }
    }

    public IEnumerable<ShellNodeConfigSection> Sections
    {
        get { return PersistedItems.OfType<ShellNodeConfigSection>().Concat(PersistedItems.OfType<ShellNodeConfigSectionPointer>().Select(p => p.SourceItem)); }
        set { }
    }

    public IEnumerable<ShellNodeConfigInput> InputSlots
    {
        get { return PersistedItems.OfType<ShellNodeConfigInput>().Concat(PersistedItems.OfType<ShellNodeConfigInputPointer>().Select(p => p.SourceItem)); }
        set { }
    }

    public IEnumerable<ShellNodeConfigOutput> OutputSlots
    {
        get { return PersistedItems.OfType<ShellNodeConfigOutput>().Concat(PersistedItems.OfType<ShellNodeConfigOutputPointer>().Select(p => p.SourceItem)); }
        set { }
    }

    public IEnumerable<IShellNodeConfigItem> IncludedInSections
    {
        get { return this.OutputsTo<IShellNodeConfigItem>(); }
    }

    public string TypeName
    {
        get { return Name.Clean(); }
        set
        {
            
        }
    }

    [InspectorProperty]
    public bool IsGraphType
    {
        get { return this["Graph Type"]; }
        set { this["Graph Type"] = value; }
    }

    public IEnumerable<ShellNodeConfig> SubNodes
    {
        get { return this.FilterNodes.OfType<ShellNodeConfig>().Where(p=>p != this); }
    }
}