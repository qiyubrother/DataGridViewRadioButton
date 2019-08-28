Build a Custom RadioButton Cell and Column for the DataGridView Control
02/03/2012
15 minutes to read
 

Régis Brid
Microsoft Corporation

March 2006

Applies to:
   Microsoft .NET Framework 2.0
   Microsoft Visual Studio 8.0

Summary: In Visual Studio 8.0, Windows Forms comes with a new grid control called the DataGridView. See how to build a custom cell and column based on the Windows Forms RadioButton control for use in the DataGridView control. (17 printed pages)

Download the associated code sample, DGV_RDB.exe.

Contents
Introduction
Characteristics of the Cell
Cell and Column Classes
Cell Implementation Details
Column Implementation Details
Screenshot of Two DataGridViewRadioButtonColumn Columns
Conclusion

Introduction
Windows Forms 2.0 offers several cell and column types for its DataGridView control. For example, the product ships with a text-box–based cell and column (DataGridViewTextBoxCell/DataGridViewTextBoxColumn) and a check-box–based duo (DataGridViewCheckBoxCell/DataGridViewCheckBoxColumn) among others. Even though the product comes with a rich set of cell and column types, some developers may want to create their own cell and column types to extend the functionality of the grid. Thankfully the DataGridView control architecture is extensible enough that such custom cells and columns can be built and used in the grid. This document explains how to create and use a cell and column that easily let the user pick an entry among multiple choices.

Characteristics of the Cell
The custom cell, called DataGridViewRadioButtonCell, reuses the look of the Windows Forms RadioButton control. It easily lets the user set its value by selecting one entry among a predefined set of choices.

Note   The implementation of the custom cell depends on Visual Styles being turned on by the operating system and the Windows Forms application (i.e., the application calls Application.EnableVisualStyles()). Indeed the custom cell's implementation uses the ScrollBarRenderer class that requires Visual Styles to be turned on.

Deriving from the combobox cell type
Because the DataGridViewRadioButtonCell cell type is close in functionality to the standard DataGridViewComboBoxCell cell type, it makes sense to derive from that class. By doing this, the custom cell can take advantage of numerous characteristics of its base class and the work involved in creating the cell is significantly reduced. It is possible, however, to directly derive from the DataGridViewCell type that is the base type of all cells provided in Windows Forms 2.0.

Editing experience
The cells used by the DataGridView control can be classified into three categories depending on editing experience.

Cells without editing experience. These are read-only cells that don't accept user input. The standard DataGridViewButtonCell is an example.
Cells with simple editing experience. These are cells that accept limited user input such as the standard DataGridViewCheckBoxCell. For that cell, the only user interactions that change the cell value are clicking the checkbox or hitting the spacebar.
Cells with complex editing experience. These are cells that provide a rich user interaction for changing their value. They require a Windows Forms control to be shown to enable that complex user input. Examples are the DataGridViewTextBoxCell cell and the DataGridViewComboBoxCell.
Since the DataGridViewRadioButtonCell cell provides a simple user interaction, it falls into the second category. It does not require a control to be brought up when the user wants to change the value of the cell. All cell types that have a simple editing experience need to implement the IDataGridViewEditingCell interface. This standardizes the interactions between the grid and the cell.

Cell and Column Classes
The minimum requirement for being able to use a custom cell type is to develop one class for that cell type. If it has a rich editing experience (case #3 above) and can't use one of the standard editing controls, DataGridViewTextBoxEditingControl and DataGridViewComboBoxEditingControl, then a second class needs to be created for the custom editing control. Finally the creation of a custom column class is optional since any cell type can be used in any column type or the base DataGridViewColumn (because columns can be heterogeneous). For example, the DataGridViewRadioButtonCell cell could be used in a DataGridViewLinkColumn or DataGridViewColumn column. Creating a special column type however makes it easier in many cases to use the custom cell type. The custom column typically replicates the specific properties of the custom cell. For the DataGridViewRadioButtonCell type, three classes were created in three files:

DataGridViewRadioButtonCell, in DataGridViewRadioButtonCell.cs, defines the custom cell type.
DataGridViewRadioButtonCellLayout, in DataGridViewRadioButtonCellLayout.cs, is a small class used by the cell to store layout information.
DataGridViewRadioButtonColumn, in DataGridViewRadioButtonColumn.cs, defines the custom column type.
All the classes were put in the DataGridViewRadioButtonElements namespace.

Cell Implementation Details
Let's focus first on how to create the custom DataGridViewRadioButtonCell cell itself.

Class definition and constructor
As mentioned earlier, the DataGridViewRadioButtonCell class derives from the DataGridViewComboBoxCell class found in Windows Forms 2.0, and implemented the IDataGridViewEditingCell interface.

namespace DataGridViewRadioButtonElements
{
public class DataGridViewRadioButtonCell : 
                 DataGridViewComboBoxCell, IDataGridViewEditingCell
    {
        public DataGridViewRadioButtonCell()
        {
            ...
        }
    }
}
Defining custom cell properties
The DataGridViewRadioButtonCell class defines a custom property called MaxDisplayedItems. It represents the maximum number of radio buttons shown by the cell.

This custom property is implemented as follows:

public class DataGridViewRadioButtonCell : DataGridViewComboBoxCell, IDataGridViewEditingCell
{
    private int maxDisplayedItems;       // Caches the value of the MaxDisplayedItems property

    public DataGridViewRadioButtonCell()
    {
        ...
        this.maxDisplayedItems = DATAGRIDVIEWRADIOBUTTONCELL_defaultMaxDisplayedItems;
    }

    [
        DefaultValue(DATAGRIDVIEWRADIOBUTTONCELL_defaultMaxDisplayedItems)
    ]
    public int MaxDisplayedItems
    {
        get
        {
            return this.maxDisplayedItems;
        }
        set
        {
            if (value < 1 || value > 100)
            {
                throw new ArgumentOutOfRangeException("MaxDisplayedItems");
            }
            this.maxDisplayedItems = value;

            if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
            {
                if (this.RowIndex == -1)
                {
                    // Invalidate and autosize column
                    this.DataGridView.InvalidateColumn(this.ColumnIndex);

                    // TODO: Add code to autosize the cell's column, the rows, the column headers 
                    // and the row headers depending on their autosize settings.
                    // The DataGridView control does not expose a public method that takes care of this.
                }
                else
                {
                    // The DataGridView control exposes a public method called UpdateCellValue
                    // that invalidates the cell so that it gets repainted and also triggers all
                    // the necessary autosizing: the cell's column and/or row, the column headers
                    // and the row headers are autosized depending on their autosize settings.
                    this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
                }
            }
        }
    }

    internal int MaxDisplayedItemsInternal
    {
        set
        {
            Debug.Assert(value >= 1 && value <= 100);
            this.maxDisplayedItems = value;
        }
    }
 }
Note   The MaxDisplayedItemsInternal property is set from the DataGridViewRadioButtonColumn class for performance reasons.

Regarding the TODO comment about autosizing features, the custom cell or column may want to automatically autosize the affected grid elements, like the standard cells and columns do. The MaxDisplayedItems property has an effect on the rendering of the cell and its preferred size. As a result, changing the property value may require adjusting the column's width, some rows' height, the column headers' height, and the row headers' width. All depends on their individual autosizing settings.

For example if the inherited autosize mode of the owning column is DataGridViewAutoSizeColumnMode.AllCells, then the protected DataGridView.AutoResizeColumn(int columnIndex, DataGridViewAutoSizeColumnMode autoSizeColumnMode, bool fixedHeight) method needs to be called with the parameter DataGridViewAutoSizeColumnMode.AllCells. Similarly, if the DataGridView.ColumnHeadersHeightSizeMode property is set to DataGridViewColumnHeadersHeightSizeMode.AutoSize, then the protected DataGridView.AutoResizeColumnHeadersHeight(int columnIndex, bool fixedRowHeadersWidth, bool fixedColumnWidth) method needs to be called, etc.

Because these methods are protected, this automatic autosizing can only be accomplished correctly when deriving from the DataGridView control. A custom cell or column type can then call a public method on the derived control that does the automatic autosizing job:

Calling from a custom cell:

    ((MyDataGridView) this.DataGridView).OnGlobalColumnAutoSize(this.ColumnIndex);
Or calling from a custom column:
    ((MyDataGridView) this.DataGridView).OnGlobalColumnAutoSize(this.Index);
The derived MyDataGridView class defines a public method OnGlobalColumnAutoSize(int columnIndex) that calls AutoResizeColumn(...), AutoResizeColumnHeadersHeight(...), AutoResizeRows(...), and AutoResizeRowHeadersWidth(...) as needed.

Tip   Getting the OnGlobalColumnAutoSize(int columnIndex) method right requires a lot of work and could be the subject of an article by itself.

Here's an implementation that takes advantage of the fact that changing the column's DefaultCellStyle property internally triggers all the autosizing adjustments.

        // MyDataGridView.OnGlobalColumnAutoSize implementation
        public void OnGlobalColumnAutoSize(int columnIndex)
        {
            if (columnIndex < -1 || columnIndex >= this.Columns.Count)
            {
                throw new ArgumentOutOfRangeException("columnIndex");
            }
            OnColumnDefaultCellStyleChanged(new DataGridViewColumnEventArgs(this.Columns[columnIndex]));
        }
Key properties to override
When creating a custom cell type for the DataGridView control, the following base properties from the DataGridViewCell class often need to be overridden.

The EditType property
The EditType property points to the type of the editing control associated with the cell. The default implementation in DataGridViewCell returns the System.Windows.Forms.DataGridViewTextBoxEditingControl type. Cell types that have no editing experience or have a simple editing experience (i.e., don't use an editing control) must override this property and return null. Cell types that have a complex editing experience must override this property and return the type of their editing control.

Implementation for the DataGridViewRadioButtonCell class:

public class DataGridViewRadioButtonCell : DataGridViewTextBoxCell, IDataGridViewEditingCell
{
    public override Type EditType
    {
        get
        {
            // Return null since no editing control is used for the editing experience.
            return null;
        }
    }
}
The FormattedValueType property
The FormattedValueType property represents the type of the data displayed on the screen, i.e., the type of the cell's FormattedValue property. For example, for the DataGridViewTextBoxCell class it is System.String, as for a DataGridViewImageCell it is System.Drawing.Image or System.Drawing.Icon. The DataGridViewRadioButtonCell displays text on the screen, so its FormattedValueType is System.String, which is the same as the base class DataGridViewComboBoxCell. So this particular property does not need to be overridden here.

The ValueType property
The ValueType property represents the type of the underlying data, i.e., the type of the cell's Value property. The DataGridViewRadioButtonCell class stores values of type System.Object by default. This is the same as the base class DataGridViewComboBoxCell. So, again, this particular property does not need to be overridden.

Key methods to override
When developing a custom cell type, it is also critical to check if the following virtual methods need to be overridden.

The Clone() method
The DataGridViewCell base class implements the ICloneable interface. Each custom cell type typically needs to override the Clone() method to copy its custom properties. Cells are cloneable because a particular cell instance can be used for multiple rows in the grid. This is the case when the cell belongs to a shared row. When a row gets unshared, its cells need to be cloned. Following is the implementation for the DataGridViewRadioButtonCell:

public override object Clone()
{
    DataGridViewRadioButtonCell dataGridViewCell = base.Clone() as DataGridViewRadioButtonCell;
    if (dataGridViewCell != null)
    {
        dataGridViewCell.MaxDisplayedItems = this.MaxDisplayedItems;
    }
    return dataGridViewCell;
}
The ContentClickUnsharesRow and OnContentClick methods
A cell's OnContentClick virtual method is called when the user clicks on its content based on the implementation of GetContentBounds(...). In the radio button cell case, the edited value may have to be updated because the user may have clicked on a radio button glyph. When a custom cell overrides OnContentClick, it may also have to override ContentClickUnsharesRow. The grid control calls ContentClickUnsharesRow before calling OnContentClick. ContentClickUnsharesRow needs to return true if the call to OnContentClick must not be made while the owning row is shared. When the response is true, the grid control makes sure the owning row is unshared before its call to OnContentClick. Typically ContentClickUnsharesRow returns true whenever the implementation of OnContentClick needs to affect a member variable of the cell.

The implementations are as follows:

    protected override bool ContentClickUnsharesRow(DataGridViewCellEventArgs e)
    {
        Point ptCurrentCell = this.DataGridView.CurrentCellAddress;
        return ptCurrentCell.X == this.ColumnIndex &&
               ptCurrentCell.Y == e.RowIndex &&
               this.DataGridView.IsCurrentCellInEditMode;
    }

    protected override void OnContentClick(DataGridViewCellEventArgs e)
    {
        if (this.DataGridView == null)
        {
            return;
        }
        Point ptCurrentCell = this.DataGridView.CurrentCellAddress;
        if (ptCurrentCell.X == this.ColumnIndex &&
            ptCurrentCell.Y == e.RowIndex &&
            this.DataGridView.IsCurrentCellInEditMode)
        {
            if (mouseLocationCode >= 0 && 
                UpdateFormattedValue(this.layout.FirstDisplayedItemIndex + mouseLocationCode, e.RowIndex))
            {
                NotifyDataGridViewOfValueChange();
            }
        }
    }
ContentClickUnsharesRow returns true whenever the implementation of OnContentClick has a chance of changing a class member like this.selectedItemIndex. If ContentClickUnsharesRow did not return true, then OnContentClick could for example affect the this.selectedItemIndex member of a shared cell, which is bad.

Other methods that handle the mouse and keyboard inputs
In addition to the ContentClickUnsharesRow/OnContentClick duo, the DataGridViewRadioButtonCell class also overrides these sets of methods to control the behavior of the cell according to the mouse and keyboard inputs:

protected override bool EnterUnsharesRow(int rowIndex, bool throughMouseClick)
protected override void OnEnter(int rowIndex, bool throughMouseClick)

protected override bool KeyDownUnsharesRow(KeyEventArgs e, int rowIndex)
protected override void OnKeyDown(KeyEventArgs e, int rowIndex)

protected override bool KeyUpUnsharesRow(KeyEventArgs e, int rowIndex)
protected override void OnKeyUp(KeyEventArgs e, int rowIndex)

protected override bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)

protected override bool MouseLeaveUnsharesRow(int rowIndex)
protected override void OnMouseLeave(int rowIndex)

protected override bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)

protected override void OnMouseEnter(int rowIndex)
protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
The two latter functions never update a class member so their associated MouseEnterUnsharesRow and MouseMoveUnsharesRow do not need to be overridden.

The Paint(Graphics, Rectangle, Rectangle, int, DataGridViewElementStates, object, object, string, DataGridViewCellStyle, DataGridViewAdvancedBorderStyle, DataGridViewPaintParts) method
This method is a critical one for any cell type. It is responsible for painting the cell. The DataGridViewRadioButtonCell's Paint implementation delegates the work to a key method of the class: ComputeLayout. That method is heavily used throughout the class since it is called by Paint, GetContentBounds, and GetMouseLocationCode. It's responsible for building a DataGridViewRadioButtonCellLayout object and optionally painting the cell.

The GetPreferredSize(Graphics, DataGridViewCellStyle, int, Size) method
Whenever a grid autosizing feature is invoked, some cells are asked to determine their preferred height, width, or size. Custom cell types can override the GetPreferredSize method to calculate their preferred dimensions based on their content, style, properties, etc. The DataGridViewRadioButtonCell class determines its preferred size based on the Items collection, the need for scroll buttons, and the style among other criterion.

The GetErrorIconBounds(Graphics, DataGridViewCellStyle, int) method
Custom cell types can override this method to customize the location of their error icon. By default, the error icon is shown close to the right edge of the cell. This default behavior is acceptable for the DataGridViewRadioButtonCell class, so it does not override this method.

The ToString() method
This method returns a compact string representation of the cell. The DataGridViewRadioButtonCell's implementation follow the standard cells' standard.

public override string ToString()
{
return "DataGridViewRadioButtonCell { ColumnIndex=" + ColumnIndex.ToString(CultureInfo.CurrentCulture) + ", 
                                      RowIndex=" + RowIndex.ToString(CultureInfo.CurrentCulture) + " }";
}
IDataGridViewEditingCell interface implementation details
The DataGridViewRadioButtonCell class implements that interface via virtual properties and methods so that developers can derive from it and customize the behavior.

The properties and methods are:

        /// Represents the cell's current formatted value
        public virtual object EditingCellFormattedValue
 
        /// Keeps track of whether the cell's value has changed or not.
        public virtual bool EditingCellValueChanged

        /// Returns the current formatted value of the cell
        public virtual object GetEditingCellFormattedValue(DataGridViewDataErrorContexts context)

        /// Called by the grid when the cell enters editing mode. 
        public virtual void PrepareEditingCellForEdit(bool selectAll)
The grid typically calls these methods to support the simple editing of the cell.

Notifying the grid of value changes
On top of implementing the IDataGridViewEditingCell interface, an editing cell typically needs to forward the fact that its content changed to the grid via the DataGridView.NotifyCurrentCellDirty(...) method. Often an editing cell must override protected virtual methods to control the content changes and be able to forward the information to the grid. In this particular case, the DataGridViewRadioButtonCell overrides two methods:

protected override void OnContentClick(DataGridViewCellEventArgs e) 
protected override void OnKeyDown(KeyEventArgs e, int rowIndex)
protected override void OnKeyUp(KeyEventArgs e, int rowIndex)
Limitations of the cell type
This implementation of a custom cell type is provided as guidance and it is far from being complete. For example, it does not support alignment characteristics (cellStyle.Alignment is ignored), right-to-left display (dataGridView.RightToLeft is ignored), high contrast display, access to the scroll button via the keyboard, or fast scrolling via the PageUp/PageDown keys. All this can be accomplished by extending the existing code.

Column Implementation Details
As mentioned earlier, the creation of a custom column type is optional. The DataGridViewRadioButtonCell can be used by any column type, including the base DataGridViewColumn type:

DataGridViewColumn dataGridViewColumn = new DataGridViewColumn(new DataGridViewRadioButtonElements.DataGridViewRadioButtonCell());
...
DataGridViewRadioButtonCell dataGridViewRadioButtonCell = dataGridViewColumn.CellTemplate as DataGridViewRadioButtonCell;
dataGridViewRadioButtonCell.MaxDisplayedItems = 5;
Custom columns typically expose the special properties of the cell type they're associated with. For example, the DataGridViewRadioButtonColumn exposes the DataSource, DisplayMember, Items, MaxDisplayedItems and ValueMember properties.

Class definition and constructor
The DataGridViewRadioButtonColumn class simply derives from the DataGridViewColumn class and its constructor uses a default DataGridViewRadioButtonCell for the cell template.

namespace DataGridViewRadioButtonElements
{
    public class DataGridViewRadioButtonColumn : DataGridViewColumn
    {
        public DataGridViewRadioButtonColumn() : base(new DataGridViewRadioButtonCell())
        {
        }
    }
}
Defining column properties
Let's take a closer look at how a column type typically implements a property. The MaxDisplayedItems property of the DataGridViewRadioButtonColumn class is implemented as follows for example:

[
    Category("Behavior"),
    DefaultValue(DataGridViewRadioButtonCell.DATAGRIDVIEWRADIOBUTTONCELL_defaultMaxDisplayedItems),
    Description("The maximum number of radio buttons to display in the cells of the column.")
]
public int MaxDisplayedItems
{
    get
    {
        if (this.RadioButtonCellTemplate == null)
        {
            throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
        }
        return this.RadioButtonCellTemplate.MaxDisplayedItems;
    }
    set
    {
        if (this.MaxDisplayedItems != value)
        {
            this.RadioButtonCellTemplate.MaxDisplayedItems = value;
            if (this.DataGridView != null)
            {
                DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                int rowCount = dataGridViewRows.Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    DataGridViewRadioButtonCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewRadioButtonCell;
                    if (dataGridViewCell != null)
                    {
                        dataGridViewCell.MaxDisplayedItemsInternal = value;
                    }
                }
                this.DataGridView.InvalidateColumn(this.Index);
                // TODO: Add code to autosize the column and rows, the column headers,
                // the row headers, depending on the autosize settings of the grid.
                // The DataGridView control does not expose a public method that takes care of this.
            }
        }
    }
}
Note   See the remarks about how to autosize grid elements in the above description of the cell's MaxDisplayedItems property.

Besides the DataSource, DisplayMember, Items, MaxDisplayedItems and ValueMember properties, the column is also defining the critical CellTemplate property as follows:

[
    Browsable(false),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
]
public override DataGridViewCell CellTemplate
{
    get
    {
        return base.CellTemplate;
    }
    set
    {
        DataGridViewRadioButtonCell dataGridViewRadioButtonCell = value as DataGridViewRadioButtonCell;
        if (value != null && dataGridViewRadioButtonCell == null)
        {
            throw new InvalidCastException("Value provided for CellTemplate must be of type 
                                            DataGridViewRadioButtonElements.DataGridViewRadioButtonCell or derive from it.");
        }
        base.CellTemplate = value;
    }
}
The CellTemplate property is used for example when the DataGridViewRowCollection.Add() gets called. Because no explicit cells are provided, a clone of the DataGridView.RowTemplate is added. By default, the RowTemplate is populated with clones of each column's CellTemplate.

Key methods to override
A custom column typically overrides the ToString() method.

// Returns a standard compact string representation of the column.
public override string ToString()
{
    StringBuilder sb = new StringBuilder(64);
    sb.Append("DataGridViewRadioButtonColumn { Name=");
    sb.Append(this.Name);
    sb.Append(", Index=");
    sb.Append(this.Index.ToString(CultureInfo.CurrentCulture));
    sb.Append(" }");
    return sb.ToString();
}
In some rare cases, the column type may want to expose a property that has no equivalent property at the cell level. Examples of that are DataGridViewLinkColumn.Text and DataGridViewImageColumn.Image. In those cases, the column class needs to override the Clone method to copy over that property.

// The custom Clone implementation needs to copy over all the specific properties of the column that 
// the associated cell does not expose.
public override object Clone()
{
    DataGridViewXXXColumn dataGridViewColumn = base.Clone() as DataGridViewXXXColumn;
    if (dataGridViewColumn != null)
    {
        dataGridViewColumn.YYY = this.YYY;
    }
    return dataGridViewColumn;
}
The DataGridViewRadioButtonColumn does not have such a property so it does not need to override the Clone method.

Screenshot of Two DataGridViewRadioButtonColumn Columns
Figure 1 is a screenshot of the sample application that makes use of the custom cell and column.

![](https://docs.microsoft.com/en-us/previous-versions/images/aa730882.winf_radiobutton_1%28en-us%2cvs.80%29.gif)

Figure1. Two DataGridViewRadioButtonColumn columns with a few cells

Conclusion
In this article, you learned how to build a custom cell and column based on the Windows Forms RadioButton control for use in the DataGridView control.
