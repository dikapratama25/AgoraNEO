Imports System
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Drawing
Imports System.Configuration

Namespace AgoraNEO

    Public Class AppBaseClass : Inherits System.Web.UI.Page
        '//To declare public variable that to be by every page
        Public strPageId As String 'for page access right
        Public strNewCSS As String
        Public intPageRecordCnt As Int32 = 0

        'Public intPageRecordCnt2 As Int32 = 0
        'Public intPageRecordCnt3 As Int32 = 0
        'Public intPageRecordCnt4 As Int32 = 0

        Public htPageAccess As New Hashtable
        Public blnSorting As Boolean = True
        Public blnPaging As Boolean = True
        Public blnCheckBox As Boolean = True
        Public blnFooter As Boolean = True
        Public blnCanAdd As Boolean = False
        Public blnCanUpdate As Boolean = False
        Public blnCanDelete As Boolean = False
        Public strCallFrom As String
        Dim dDispatcher As New AgoraNEO.dispatcher
        '//End
        Protected Overridable Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim strURL As String, strCSSPath As String
            strPageId = Page.Request.QueryString("pageid")

            'Michelle (12/10/2012) 
            LogUserSreenAct(sender.Request.Path & "[" & sender.ClientQueryString & "]")

            '//Add StyleSheet dynamicly, styleSheet can be customised by company
            '//RAppCommon From DataBase

            'strCSSPath = Request.ApplicationPath &  "/css/STYLES.CSS"            
            If strNewCSS = "true" Then
                strCSSPath = dDispatcher.direct("Plugins/CSS", "SPP.CSS")
                Response.Write("<link type='text/css' rel='stylesheet' href='" & strCSSPath & "'>")
            ElseIf strNewCSS = "both" Then
                strCSSPath = dDispatcher.direct("Plugins/CSS", "STYLES.CSS")
                Response.Write("<link type='text/css' rel='stylesheet' href='" & strCSSPath & "'>")
                strCSSPath = dDispatcher.direct("Plugins/CSS", "SPP.CSS")
                Response.Write("<link type='text/css' rel='stylesheet' href='" & strCSSPath & "'>")
            Else
                strCSSPath = dDispatcher.direct("Plugins/CSS", "STYLES.CSS")
                Response.Write("<link type='text/css' rel='stylesheet' href='" & strCSSPath & "'>")
            End If
            'Response.Redirect(dDispatcher.direct("DO", "AddDO.aspx", "coyID=" & vendorID & "&coyName=" & vendorName))
            strNewCSS = ""

            'strCSSPath = "STYLES.CSS"
            '//if use this,stylesheet lost after clicking og back button
            'RegisterClientScriptBlock("MyCSS", String.Format("<link rel='stylesheet' type='text/css' href='" & strCSSPath & "'>"))
            '<LINK href="../css/Styles.css" rel="stylesheet">
            'Response.Write("<link type='text/css' rel='stylesheet' href='" & strCSSPath & "'>")
            '//End

            'Redirect to Unauthorized when no page id pass in
            'strURL = "http://" & Request.ServerVariables("HTTP_Host") & "/" & _
            '         Request.ApplicationPath & "/Unauthorized.aspx"
            If strPageId = String.Empty Then
                '    Dim vbs As String
                '    vbs = vbs & "<script language=""vbs"">"
                '    vbs = vbs & vbLf & "parent.location=""" & strURL & """"
                '    vbs = vbs & "</script>"
                '    Response.Write(vbs)
                '    Exit Sub
            End If

            '//Add HTTPHAppCommoner to redirect to login after timmeout
            If ConfigurationManager.AppSettings("SSLHttp") Then
                strURL = "https://" & Request.ServerVariables("HTTP_Host") & "/"
            Else
                strURL = "http://" & Request.ServerVariables("HTTP_Host") & "/"
            End If
            strURL &= Request.ApplicationPath & "/Common/Initial/TimeOut.aspx?pageid=" & strPageId

            If CStr(Session("UserId")) = String.Empty Then
                Response.Redirect(strURL)
            End If
            strCallFrom = Request.ServerVariables("Path_Info") & "?" & Request.ServerVariables("QUERY_STRING")
            Response.AddHeader(" REFRESH", " " & CStr(CInt(Session.Timeout + 1) * 60) & ";url=" & strCallFrom)

            '//Temporary Remark
            'Response.Expires = -1
            'Response.AddHAppCommoner("cache-control", "private")
            'Response.AddHAppCommoner("pragma", "no-cache")
            'Response.CacheControl = "no-cache"


            '******* Write all key in Request.ServerVariable *
            'Dim loop1, loop2 As Integer
            'Dim arr1(), arr2() As String
            'Dim coll As System.Collections.Specialized.NameValueCollection

            '' Load ServerVariable collection into NameValueCollection object.
            'coll = Request.ServerVariables
            '' Get names of all keys into a string array.
            'arr1 = coll.AllKeys
            'For loop1 = 0 To arr1.GetUpperBound(0)
            '    Response.Write("Key: " & arr1(loop1) & "<br>")
            '    arr2 = coll.GetValues(loop1) ' Get all values under this key.
            '    For loop2 = 0 To arr2.GetUpperBound(0)
            '        Response.Write("Value " & CStr(loop2) & ": " & arr2(loop2) & "<br>")
            '    Next loop2
            'Next loop1
            '******* Write all key in Request.ServerVariable *

            'strPageId = "123"
            'CheckButtonAccess()
        End Sub

#Region " DataGrid Function"
        Public Sub SetGridProperty(ByRef pDataGrid As DataGrid, Optional ByVal _showPaging As String = "")
            With pDataGrid
                .AutoGenerateColumns = False
                .AllowCustomPaging = False
                .AllowPaging = blnPaging
                If Not _showPaging = "" Then
                    If _showPaging = "Y" Then .AllowPaging = True Else .AllowPaging = False
                End If
                .AllowSorting = blnSorting
                .CssClass = "Grid"
                .ShowFooter = blnPaging
                If Not _showPaging = "" Then
                    If _showPaging = "Y" Then .ShowFooter = True Else .ShowFooter = False
                End If
                .ShowHeader = True
                If blnPaging Then
                    If Not Session("PageCount") Is Nothing Then
                        .PageSize = Session("PageCount")
                    Else
                        .PageSize = 10
                    End If
                End If
                '.Font.Size = System.Web.UI.WebControls.FontUnit.Point(30)
                .CellPadding = -1
                .CellSpacing = 0
                '.Width = System.Web.UI.WebControls.Unit.Percentage(100)
                .GridLines = GridLines.Both
                '.HorizontalAlign = HorizontalAlign.Center
            End With


            With pDataGrid.EditItemStyle
                .Font.Bold = True
                '.Font.Name = "Verdana"
                '.Font.Size = System.Web.UI.WebControls.FontUnit.Point(8)
            End With


            With pDataGrid.HeaderStyle
                .CssClass = "GridHeader"
                .HorizontalAlign = HorizontalAlign.Left
                '.ForeColor = Color.White
                '.Font.Size = System.Web.UI.WebControls.FontUnit.Point(9)
                '.Font.Bold = True
            End With

            'With pDataGrid.FooterStyle
            '    .Font.Bold = True
            '    .Font.Name = "Verdana"
            '    .Font.Size = System.Web.UI.WebControls.FontUnit.Point(9)
            'End With

            If blnPaging Then
                With pDataGrid.PagerStyle
                    .HorizontalAlign = HorizontalAlign.Right
                    .VerticalAlign = VerticalAlign.Middle
                    .CssClass = "gridPager"
                    '.Height = System.Web.UI.WebControls.Unit.Pixel(1)
                    .Mode = PagerMode.NumericPages
                    .PrevPageText = "Next"
                    .NextPageText = "Previous"
                    '.ForeColor = Color.Black
                End With
            End If

            'With pDataGrid.ItemStyle
            '    .Font.Name = "Verdana"
            '    .Font.Size = System.Web.UI.WebControls.FontUnit.Point(9)
            'End With

            With pDataGrid.AlternatingItemStyle
                '.Font.Name = "Verdana"
                '.Font.Size = System.Web.UI.WebControls.FontUnit.Point(9)
                .BackColor = Color.FromName("#f5f9fc")
            End With

            'With pDataGrid.SelectedItemStyle
            '    .HorizontalAlign = HorizontalAlign.Left
            '    .VerticalAlign = VerticalAlign.Middle
            '    .Height = System.Web.UI.WebControls.Unit.Pixel(1)
            '    .ForeColor = Color.Black
            '    .Font.Bold = True
            'End With
        End Sub

        Public Sub SetDashboardGridProperty(ByRef pDataGrid As DataGrid)
            With pDataGrid
                .AutoGenerateColumns = False
                .AllowCustomPaging = False
                .AllowPaging = blnPaging
                .AllowSorting = blnSorting
                .CssClass = "Grid"
                .ShowFooter = blnPaging
                .ShowHeader = True
                If blnPaging Then
                    .PageSize = 3
                End If
                '.Font.Size = System.Web.UI.WebControls.FontUnit.Point(30)
                .CellPadding = -1
                .CellSpacing = 0
                '.Width = System.Web.UI.WebControls.Unit.Percentage(100)
                .GridLines = GridLines.Both
                '.HorizontalAlign = HorizontalAlign.Center
            End With


            With pDataGrid.EditItemStyle
                .Font.Bold = True
                '.Font.Name = "Verdana"
                '.Font.Size = System.Web.UI.WebControls.FontUnit.Point(8)
            End With


            With pDataGrid.HeaderStyle
                .CssClass = "GridHeader"
                .HorizontalAlign = HorizontalAlign.Left
                '.ForeColor = Color.White
                '.Font.Size = System.Web.UI.WebControls.FontUnit.Point(9)
                '.Font.Bold = True
            End With

            'With pDataGrid.FooterStyle
            '    .Font.Bold = True
            '    .Font.Name = "Verdana"
            '    .Font.Size = System.Web.UI.WebControls.FontUnit.Point(9)
            'End With

            If blnPaging Then
                With pDataGrid.PagerStyle
                    .HorizontalAlign = HorizontalAlign.Right
                    .VerticalAlign = VerticalAlign.Middle
                    .CssClass = "gridPager"
                    '.Height = System.Web.UI.WebControls.Unit.Pixel(1)
                    .Mode = PagerMode.NumericPages
                    .PageButtonCount = 5
                    .PrevPageText = "Next"
                    .NextPageText = "Previous"
                    '.ForeColor = Color.Black
                End With
            End If

            'With pDataGrid.ItemStyle
            '    .Font.Name = "Verdana"
            '    .Font.Size = System.Web.UI.WebControls.FontUnit.Point(9)
            'End With

            With pDataGrid.AlternatingItemStyle
                '.Font.Name = "Verdana"
                '.Font.Size = System.Web.UI.WebControls.FontUnit.Point(9)
                .BackColor = Color.FromName("#f5f9fc")
            End With

            'With pDataGrid.SelectedItemStyle
            '    .HorizontalAlign = HorizontalAlign.Left
            '    .VerticalAlign = VerticalAlign.Middle
            '    .Height = System.Web.UI.WebControls.Unit.Pixel(1)
            '    .ForeColor = Color.Black
            '    .Font.Bold = True
            'End With
        End Sub

        Public Sub Grid_SortCommand(ByVal pDataGrid As DataGrid, ByVal e As DataGridSortCommandEventArgs)
            Dim strSortBy As String = ViewState("SortExpression")
            Dim strSortAscending As String = ViewState("SortAscending")

            ViewState("SortExpression") = e.SortExpression
            If e.SortExpression = strSortBy Then
                ViewState("SortAscending") = IIf(ViewState("SortAscending") = "yes", "no", "yes")
            Else
                ViewState("SortAscending") = "no"
            End If
        End Sub

        Public Sub Grid_SortCommand_WID(ByVal pDataGrid As DataGrid, ByVal e As DataGridSortCommandEventArgs)
            Dim strSortBy As String = ViewState("SortExpression_" & pDataGrid.ID)
            Dim strSortAscending As String = ViewState("SortAscending")

            ViewState("SortExpression") = e.SortExpression
            If e.SortExpression = strSortBy Then
                ViewState("SortAscending") = IIf(ViewState("SortAscending") = "yes", "no", "yes")
            Else
                ViewState("SortAscending") = "no"
            End If
        End Sub

        Public Sub Grid_SortCommandDashboard(ByVal pDataGrid As DataGrid, ByVal e As DataGridSortCommandEventArgs, ByVal DashboardViewState As String)
            Dim strSortBy As String = ViewState("SortExpression" & DashboardViewState)
            Dim strSortAscending As String = ViewState("SortAscending" & DashboardViewState)

            ViewState("SortExpression" & DashboardViewState) = e.SortExpression
            If e.SortExpression = strSortBy Then
                ViewState("SortAscending" & DashboardViewState) = IIf(ViewState("SortAscending" & DashboardViewState) = "yes", "no", "yes")
            Else
                ViewState("SortAscending" & DashboardViewState) = "no"
            End If
        End Sub

        Public Sub Grid_ItemCreated(ByVal pDataGrid As DataGrid, ByVal e As DataGridItemEventArgs)
            Dim iLoop As Long

            If blnSorting Then
                If e.Item.ItemType = ListItemType.Header Then


                    Dim strSortBy As String = ViewState("SortExpression")
                    Dim strSortAscending As String = ViewState("SortAscending")
                    Dim strOrder = IIf(strSortAscending = "yes", " 5", " 6")
                    Dim intCol As Integer
                    If blnCheckBox Then
                        intCol = 0
                    Else
                        intCol = 0 'testing
                    End If
                    For iLoop = intCol To pDataGrid.Columns.Count - 1
                        If pDataGrid.Columns(iLoop).SortExpression <> "" Then
                            If strSortBy = pDataGrid.Columns(iLoop).SortExpression Then
                                Dim cell As TableCell = e.Item.Cells(iLoop)
                                Dim lblSorted As New Label
                                lblSorted.Font.Name = "webdings"
                                lblSorted.Font.Size = FontUnit.XSmall
                                lblSorted.Text = strOrder
                                cell.Controls.Add(lblSorted)
                            End If
                        End If
                    Next
                End If
            End If

            If blnPaging Then
                If (e.Item.ItemType = ListItemType.Pager) Then
                    Dim pager As TableCell = e.Item.Controls(0)
                    For iLoop = 0 To pager.Controls.Count Step 2
                        'If iLoop = 0 Then
                        '    Dim t As New TextBox
                        '    t.CssClass = "txtbox"
                        '    t.Width = Unit.Pixel(50)
                        '    pager.Controls.Add(t)
                        'End If
                        Dim obj As UI.Control = pager.Controls(iLoop)
                        If TypeOf (obj) Is LinkButton Then
                            Dim h As LinkButton = obj
                            h.Text = "<b>[" & h.Text & "]</b>"
                        Else
                            Dim l As Label = obj
                            l.Text = "<b><font class='GridPager'>" & +l.Text & "</font></b>"
                        End If
                    Next
                End If
            End If


            If (e.Item.ItemType = ListItemType.Footer) Then
                Dim tcc As TableCellCollection = e.Item.Cells
                Dim intCount As Integer = tcc.Count

                For iLoop = 0 To intCount - 2
                    e.Item.Cells.RemoveAt(1)
                Next
                Dim c As TableCell = e.Item.Cells(0)
                c.ColumnSpan = intCount
                If blnPaging Then
                    c.Text = intPageRecordCnt & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                Else
                    c.Text = intPageRecordCnt & " record(s) found."
                End If

            End If

            'If (e.Item.ItemType = ListItemType.Item) Or _
            '(e.Item.ItemType = ListItemType.AlternatingItem) Then
            '    e.Item.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#cccccc';this.style.color='green';")
            '    If e.Item.ItemType = ListItemType.Item Then
            '        e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='lightyellow';this.style.color='black';")
            '    Else
            '        e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='#F1F0DC';this.style.color='black';")
            '    End If
            '    'e.Item.Attributes.Add("style", "CURSOR:hand")
            '    '  e.Item.Attributes.Add("Onclick", "selectRow(" & e.Item.ItemIndex & ");")
            'End If

        End Sub
        'Michelle (22/4/2011) - This function is same as the Grid_ItemCreated except that no Nos. of records is displayed in the datagrid footer
        Public Sub Grid_ItemCreated_WORecCnt(ByVal pDataGrid As DataGrid, ByVal e As DataGridItemEventArgs)
            Dim iLoop As Long

            If blnSorting Then
                If e.Item.ItemType = ListItemType.Header Then

                    Dim strSortBy As String = ViewState("SortExpression")
                    Dim strSortAscending As String = ViewState("SortAscending")
                    Dim strOrder = IIf(strSortAscending = "yes", " 5", " 6")
                    Dim intCol As Integer
                    If blnCheckBox Then
                        intCol = 0
                    Else
                        intCol = 0 'testing
                    End If
                    For iLoop = intCol To pDataGrid.Columns.Count - 1
                        If pDataGrid.Columns(iLoop).SortExpression <> "" Then
                            If strSortBy = pDataGrid.Columns(iLoop).SortExpression Then
                                Dim cell As TableCell = e.Item.Cells(iLoop)
                                Dim lblSorted As New Label
                                lblSorted.Font.Name = "webdings"
                                lblSorted.Font.Size = FontUnit.XSmall
                                lblSorted.Text = strOrder
                                cell.Controls.Add(lblSorted)
                            End If
                        End If
                    Next
                End If
            End If

            If blnPaging Then
                If (e.Item.ItemType = ListItemType.Pager) Then
                    Dim pager As TableCell = e.Item.Controls(0)
                    For iLoop = 0 To pager.Controls.Count Step 2
                        'If iLoop = 0 Then
                        '    Dim t As New TextBox
                        '    t.CssClass = "txtbox"
                        '    t.Width = Unit.Pixel(50)
                        '    pager.Controls.Add(t)
                        'End If
                        Dim obj As UI.Control = pager.Controls(iLoop)
                        If TypeOf (obj) Is LinkButton Then
                            Dim h As LinkButton = obj
                            h.Text = "<b>[" & h.Text & "]</b>"
                        Else
                            Dim l As Label = obj
                            l.Text = "<b><font class='GridPager'>" & +l.Text & "</font></b>"
                        End If
                    Next
                End If
            End If


            If (e.Item.ItemType = ListItemType.Footer) Then
                Dim tcc As TableCellCollection = e.Item.Cells
                Dim intCount As Integer = tcc.Count

                For iLoop = 0 To intCount - 2
                    e.Item.Cells.RemoveAt(1)
                Next
                Dim c As TableCell = e.Item.Cells(0)
                c.ColumnSpan = intCount
                'If blnPaging Then
                '    c.Text = intPageRecordCnt & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                'Else
                '    c.Text = intPageRecordCnt & " record(s) found."
                'End If
                c.Text = ""
            End If

        End Sub
        Public Sub Grid_ItemCreatedDashboard(ByVal pDataGrid As DataGrid, ByVal e As DataGridItemEventArgs, ByVal DashboardViewState As String)
            Dim iLoop As Long

            If blnSorting Then
                If e.Item.ItemType = ListItemType.Header Then

                    Dim strSortBy As String = ViewState("SortExpression" & DashboardViewState)
                    Dim strSortAscending As String = ViewState("SortAscending" & DashboardViewState)
                    Dim strOrder = IIf(strSortAscending = "yes", " 5", " 6")
                    Dim intCol As Integer
                    If blnCheckBox Then
                        intCol = 0
                    Else
                        intCol = 0
                    End If
                    For iLoop = intCol To pDataGrid.Columns.Count - 1
                        If pDataGrid.Columns(iLoop).SortExpression <> "" Then
                            If strSortBy = pDataGrid.Columns(iLoop).SortExpression Then
                                Dim cell As TableCell = e.Item.Cells(iLoop)
                                Dim lblSorted As New Label
                                lblSorted.Font.Name = "webdings"
                                lblSorted.Font.Size = FontUnit.XSmall
                                lblSorted.Text = strOrder
                                cell.Controls.Add(lblSorted)
                            End If
                        End If
                    Next
                End If
            End If

            If blnPaging Then
                If (e.Item.ItemType = ListItemType.Pager) Then
                    Dim pager As TableCell = e.Item.Controls(0)
                    For iLoop = 0 To pager.Controls.Count Step 2
                        Dim obj As UI.Control = pager.Controls(iLoop)
                        If TypeOf (obj) Is LinkButton Then
                            Dim h As LinkButton = obj
                            h.Text = "<b>[" & h.Text & "]</b>"
                        Else
                            Dim l As Label = obj
                            l.Text = "<b><font class='GridPager'>" & +l.Text & "</font></b>"
                        End If
                    Next
                End If
            End If


            If (e.Item.ItemType = ListItemType.Footer) Then
                Dim tcc As TableCellCollection = e.Item.Cells
                Dim intCount As Integer = tcc.Count

                For iLoop = 0 To intCount - 2
                    e.Item.Cells.RemoveAt(1)
                Next
                Dim c As TableCell = e.Item.Cells(0)
                c.ColumnSpan = intCount
                If blnPaging Then
                    Select Case DashboardViewState
                        Case "OutstandingPOVend"
                            If Session("PageRecordOutstandingPOVend") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOutstandingPOVend") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "OverduePOVend"
                            If Session("PageRecordOverduePOVend") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOverduePOVend") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "OutstandingRFQVend"
                            If Session("PageRecordOutstandingRFQVend") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOutstandingRFQVend") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "OutstandingRFQ"
                            If Session("PageRecordOutstandingRFQ") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOutstandingRFQ") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "OutstandingInvoiceVend"
                            If Session("PageRecordOutstandingInvoice") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOutstandingInvoice") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "PendingApprPM"
                            If Session("PageRecordPendingApprPM") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordPendingApprPM") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "PendingMyAppr"
                            If Session("PageRecordPendingMyAppr") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordPendingMyAppr") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "OutstdPO"
                            If Session("PageRecordOutstdPO") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOutstdPO") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "InPendingPymt"
                            If Session("PageRecordInPendingPymt") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordInPendingPymt") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If

                        Case "InDOSK"
                            If Session("PageRecordInDOSK") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordInDOSK") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If

                        Case "InInv"
                            If Session("PageRecordInInv") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordInInv") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "OutstandingGRNQCVerify"
                            If Session("PageRecordOutstandingGRNQCVerify") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOutstandingGRNQCVerify") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If

                        Case "PendingMyAppPR"
                            If Session("PageRecordPendingMyAppPR") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordPendingMyAppPR") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "OutstdPR"
                            If Session("PageRecordOutstdPR") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOutstdPR") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "PendingConvPR"
                            If Session("PageRecordPendingConvPR") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordPendingConvPR") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "OutstdIPPDoc"
                            If Session("PageRecordOutstdIPPDoc") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOutstdIPPDoc") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "IPPApproval"
                            If Session("PageRecordIPPApproval") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordIPPApproval") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "IPPPendingPSDSentDate"
                            If Session("PageRecordIPPPendingPSDSentDate") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordIPPPendingPSDSentDate") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "IPPPendingPSDRecvDate"
                            If Session("PageRecordIPPPendingPSDRecvDate") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordIPPPendingPSDRecvDate") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "IQCApproval"
                            If Session("PageRecordIQCApproval") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordIQCApproval") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "OutstandingIR"
                            If Session("PageRecordOutstandingIR") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOutstandingIR") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "PendingMRSAcknowledge"
                            If Session("PageRecordPendingMRSAcknowledge") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordPendingMRSAcknowledge") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "PendingMyIRApproval"
                            If Session("PageRecordPendingMyIRApproval") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordPendingMyIRApproval") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "IssueMRS"
                            If Session("PageRecordIssueMRS") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordIssueMRS") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "OutRIAck"
                            If Session("PageRecordOutRIAck") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOutRIAck") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        'Modified for IPP GST Stage 2A - CH
                        Case "OutBillDoc"
                            If Session("PageRecordOutBillDoc") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordOutBillDoc") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "PendingBillApproval"
                            If Session("PageRecordPendingBillApproval") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordPendingBillApproval") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                            '-----------------------------------

                            'Yap: 2015-02-27: Modified for Agora GST Stage 2
                        Case "FOIncomingDN"
                            If Session("PageRecordFOIncomingDN") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordFOIncomingDN") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "FMIncomingPendingDN"
                            If Session("PageRecordFMIncomingPendingDN") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordFMIncomingPendingDN") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                        Case "FMPendingAckCN"
                            If Session("PageRecordFMPendingAckCN") = 0 Then
                                c.Text = "0 record found."
                            Else
                                c.Text = Session("PageRecordFMPendingAckCN") & " record(s) found. " & pDataGrid.PageCount.ToString() & " page(s) found."
                            End If
                    End Select
                Else
                    c.Text = intPageRecordCnt & " record(s) found."
                End If

            End If

        End Sub

        Public Sub resetDatagridPageIndex(ByVal dtg As DataGrid, ByVal dvw As DataView)
            ' check when user re-enter search criteria and click on other page without click search button
            If dtg.CurrentPageIndex > (dvw.Count \ dtg.PageSize) Then
                dtg.CurrentPageIndex = IIf((dvw.Count \ dtg.PageSize) = 1, 0, (dvw.Count \ dtg.PageSize))
            ElseIf dtg.CurrentPageIndex = (dvw.Count \ dtg.PageSize) Then
                If ViewState("PageCount") = (dvw.Count \ dtg.PageSize) Then
                    'user does not re-enter search criteria 
                    dtg.CurrentPageIndex = IIf((dvw.Count \ dtg.PageSize) = 0, 0, (dvw.Count \ dtg.PageSize) - 1)
                Else
                    If (dvw.Count Mod dtg.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        ' dtg.CurrentPageIndex = IIf((dvw.Count \ dtg.PageSize) = 1, 0, (dvw.Count \ dtg.PageSize))
                        dtg.CurrentPageIndex = 0
                    Else
                        ' total record = 11, 12, ...
                        dtg.CurrentPageIndex = (dvw.Count \ dtg.PageSize)
                    End If
                End If
            End If
        End Sub
        'Michelle (28/2/2011) - reset the page after manually remove the row in the datagrid
        Public Sub resetDatagridPageIndexM(ByVal dtg As DataGrid, ByVal recCnt As Integer)
            ' check when user re-enter search criteria and click on other page without click search button
            If dtg.CurrentPageIndex > (recCnt \ dtg.PageSize) Then
                dtg.CurrentPageIndex = IIf((recCnt \ dtg.PageSize) = 1, 0, (recCnt \ dtg.PageSize))
            ElseIf dtg.CurrentPageIndex = (recCnt \ dtg.PageSize) Then
                If ViewState("PageCount") = (recCnt \ dtg.PageSize) Then
                    'user does not re-enter search criteria 
                    dtg.CurrentPageIndex = IIf((recCnt \ dtg.PageSize) = 0, 0, (recCnt \ dtg.PageSize) - 1)
                Else
                    If (recCnt Mod dtg.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtg.CurrentPageIndex = IIf((recCnt \ dtg.PageSize) = 1, 0, (recCnt \ dtg.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtg.CurrentPageIndex = (recCnt \ dtg.PageSize)
                    End If
                End If
            End If
        End Sub

        Public Sub resetDashboardDatagridPageIndex(ByVal dtg As DataGrid, ByVal dvw As DataView, ByVal DashboardViewState As String)
            Dim iPageSize As Integer = 3 'PageSize = 3

            ' check when user re-enter search criteria and click on other page without click search button
            If dtg.CurrentPageIndex > (dvw.Count \ iPageSize) Then
                dtg.CurrentPageIndex = IIf((dvw.Count \ iPageSize) = 1, 0, (dvw.Count \ iPageSize))
            ElseIf dtg.CurrentPageIndex = (dvw.Count \ iPageSize) Then
                If ViewState("PageCount" & DashboardViewState) = (dvw.Count \ iPageSize) Then
                    'user does not re-enter search criteria 
                    dtg.CurrentPageIndex = IIf((dvw.Count \ iPageSize) = 0, 0, (dvw.Count \ iPageSize) - 1)
                Else
                    If (dvw.Count Mod iPageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtg.CurrentPageIndex = IIf((dvw.Count \ iPageSize) = 1, 0, (dvw.Count \ iPageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtg.CurrentPageIndex = (dvw.Count \ iPageSize)
                    End If
                End If
            End If
        End Sub
#End Region

#Region " Copy From Security.VB"
        Private strSQL
        Dim objDcom As New EAD.DBCom  '(ConfigurationManager.AppSettings("nav"))
        Public Enum eAccessType
            View = 0
            Insert = 1
            Update = 2
            Delete = 3
        End Enum

        Public Function BuildRightHash() As Hashtable
            Dim htUserDetail As New Hashtable
            'Dim htUser As New Hashtable
            'Dim htAllDetail As New Hashtable
            Dim tDS As DataSet


            strSQL = "SELECT MM_MENU_ID, " &
            "CASE WHEN SUM(CNTview)=0 THEN 'N' ELSE 'Y' END  as sumView, " &
            "CASE WHEN SUM(CNTInsert)=0 THEN 'N' ELSE 'Y' END  as sumInsert, " &
            "CASE WHEN SUM(CNTUpdate)=0 THEN 'N' ELSE 'Y' END  as sumUpdate, " &
            "CASE WHEN SUM(CNTDelete)=0 THEN 'N' ELSE 'Y' END  as sumDelete " &
            "FROM(SELECT MM_MENU_ID, " &
            "CASE when UAR_allow_view='Y' then 1 else 0 end as cntView , " &
            "CASE when UAR_allow_insert='Y' then 1 else 0 end as cntInsert,  " &
            "CASE when UAR_allow_update='Y' then 1 else 0 end as cntUpdate, " &
            "CASE when UAR_allow_delete='Y' then 1 else 0 end as cntDelete " &
            "FROM MENU_MSTR M, USER_MSTR U,USER_ACCESS_RIGHT R ,USERS_USRGRP G " &
            "WHERE M.MM_GROUP='ehub' " &
            "AND U.UM_USER_ID=G.UU_USER_ID AND UM_COY_ID=UU_COY_ID " &
            "AND UU_USRGRP_ID=R.UAR_USRGRP_ID " &
            "AND MM_MENU_ID=UAR_MENU_ID  " &
            "AND UM_COY_ID='" & Session("CompanyId") & "' " &
            "AND G.uu_USER_ID='" & Session("UserID") & "' AND MM_MENU_ID=" & strPageId & ")a " &
            "GROUP BY MM_MENU_ID  " &
            "ORDER BY  MM_MENU_ID "
            tDS = objDcom.FillDs(strSQL)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                htUserDetail = New Hashtable
                htUserDetail.Add(0, IIf(tDS.Tables(0).Rows(j).Item("sumView") = "Y", True, False))
                htUserDetail.Add(1, IIf(tDS.Tables(0).Rows(j).Item("sumInsert") = "Y", True, False))
                htUserDetail.Add(2, IIf(tDS.Tables(0).Rows(j).Item("sumUpdate") = "Y", True, False))
                htUserDetail.Add(3, IIf(tDS.Tables(0).Rows(j).Item("sumDelete") = "Y", True, False))
            Next
            '//REMARK BY moo
            'htUser.Add(pUserId, htAllDetail)
            'Return htUser
            Return htUserDetail
        End Function

        Public Function BuildRightHash(ByVal pFixedRole() As AgoraNEO.FixedRole) As Hashtable
            Dim htUserDetail As New Hashtable
            'Dim htUser As New Hashtable
            'Dim htAllDetail As New Hashtable
            Dim tDS As DataSet

            strSQL = "SELECT MM_MENU_ID, " &
            "CASE WHEN SUM(CNTview)=0 THEN 'N' ELSE 'Y' END  as sumView, " &
            "CASE WHEN SUM(CNTInsert)=0 THEN 'N' ELSE 'Y' END  as sumInsert, " &
            "CASE WHEN SUM(CNTUpdate)=0 THEN 'N' ELSE 'Y' END  as sumUpdate, " &
            "CASE WHEN SUM(CNTDelete)=0 THEN 'N' ELSE 'Y' END  as sumDelete " &
            "FROM(SELECT MM_MENU_ID, " &
            "CASE when UAR_allow_view='Y' then 1 else 0 end as cntView , " &
            "CASE when UAR_allow_insert='Y' then 1 else 0 end as cntInsert,  " &
            "CASE when UAR_allow_update='Y' then 1 else 0 end as cntUpdate, " &
            "CASE when UAR_allow_delete='Y' then 1 else 0 end as cntDelete " &
            "FROM MENU_MSTR M, USER_MSTR U,USER_ACCESS_RIGHT R ,USERS_USRGRP G, USER_GROUP_MSTR GM " &
            "WHERE M.MM_GROUP='ehub' " &
            "AND U.UM_USER_ID=G.UU_USER_ID AND UM_COY_ID=UU_COY_ID " &
            "AND UU_USRGRP_ID=R.UAR_USRGRP_ID " &
            "AND MM_MENU_ID=UAR_MENU_ID  " &
            "AND GM.UGM_USRGRP_ID = UU_USRGRP_ID "

            Dim role As AgoraNEO.FixedRole

            strSQL &= "AND GM.UGM_FIXED_ROLE IN (''"

            For Each role In pFixedRole
                strSQL &= ", '" & role.ToString.Replace("_", " ") & "'"
            Next

            strSQL &= ")"

            strSQL &= "AND UM_COY_ID='" & Session("CompanyId") & "' " &
            "AND G.uu_USER_ID='" & Session("UserID") & "' AND MM_MENU_ID=" & strPageId & ")a " &
            "GROUP BY MM_MENU_ID  " &
            "ORDER BY  MM_MENU_ID "
            tDS = objDcom.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                htUserDetail = New Hashtable
                htUserDetail.Add(0, IIf(tDS.Tables(0).Rows(j).Item("sumView") = "Y", True, False))
                htUserDetail.Add(1, IIf(tDS.Tables(0).Rows(j).Item("sumInsert") = "Y", True, False))
                htUserDetail.Add(2, IIf(tDS.Tables(0).Rows(j).Item("sumUpdate") = "Y", True, False))
                htUserDetail.Add(3, IIf(tDS.Tables(0).Rows(j).Item("sumDelete") = "Y", True, False))
            Next
            '//REMARK BY moo
            'htUser.Add(pUserId, htAllDetail)
            'Return htUser
            Return htUserDetail
        End Function

        Public Function CheckUserRight(ByVal htRight As Hashtable, ByVal pAcessType As eAccessType) As Boolean
            If htRight Is Nothing Then
                Return False
            Else
                If htRight.Count = 0 Then
                    Return False
                Else
                    Return htRight(CInt(pAcessType))
                End If
            End If
            'Return CType(htDetail(pPageId), Hashtable)(CInt(pAcessType))
        End Function

        Public Sub CheckButtonAccess(Optional ByVal blnViewOnly As Boolean = False)
            CheckButtonAccess(blnViewOnly, Nothing)
        End Sub

        Public Sub CheckButtonAccess(ByVal ParamArray pFixedRole() As AgoraNEO.FixedRole)
            CheckButtonAccess(False, pFixedRole)
        End Sub

        Private Sub CheckButtonAccess(ByVal blnViewOnly As Boolean, ByVal ParamArray pFixedRole() As AgoraNEO.FixedRole)
            Dim myEnumerator As IDictionaryEnumerator = htPageAccess.GetEnumerator()
            Dim myEnumerator2 As IEnumerator
            Dim alButtonList As ArrayList
            Dim ParamString As String = String.Empty
            Dim blnCanAccess As Boolean
            '//view,insert,delete,update
            Dim htRight As Hashtable
            'If strPageId <> String.Empty Then
            If pFixedRole Is Nothing Then
                htRight = BuildRightHash()
            ElseIf pFixedRole.Length = 0 Then
                htRight = BuildRightHash()
            Else
                htRight = BuildRightHash(pFixedRole)
            End If

            'Else
            '    Dim vbs, strURL As String
            '    strURL = "http://" & Request.ServerVariables("HTTP_Host") & "/" & _
            '    Request.ApplicationPath & "/Unauthorized.aspx"
            '    vbs = vbs & "<script language=""vbs"">"
            '    vbs = vbs & vbLf & "parent.location=""" & strURL & """"
            '    vbs = vbs & "</script>"
            '    Response.Write(vbs)
            '    Exit Sub
            'End If
            '//view
            blnCanAccess = CheckUserRight(htRight, eAccessType.View)
            If Not blnCanAccess Then
                '//unauthorised access
                '//if cannot view, cannot do add,update.. also
                Dim vbs, strURL As String
                If ConfigurationManager.AppSettings("SSLHttp") Then
                    strURL = "https://" & Request.ServerVariables("HTTP_Host") & "/"
                Else
                    strURL = "http://" & Request.ServerVariables("HTTP_Host") & "/"
                End If
                'strURL = "http://" & Request.ServerVariables("HTTP_Host") & "/" & _
                strURL &= Request.ApplicationPath & "/Common/Initial/Unauthorized.aspx"
                vbs = vbs & "<script language=""vbs"">"
                vbs = vbs & vbLf & "parent.location=""" & strURL & """"
                vbs = vbs & "</script>"
                Response.Write(vbs)
                Exit Sub
            End If

            '//this checking is for those pages that only has datagrid but not add,delete,update button
            If blnViewOnly Then Exit Sub

            While myEnumerator.MoveNext()
                'ParamString &= "&" & myEnumerator.Key & "=" & Server.UrlEncode(myEnumerator.Value) & "<br>"
                alButtonList = CType(myEnumerator.Value, ArrayList)
                myEnumerator2 = alButtonList.GetEnumerator()
                Select Case myEnumerator.Key
                    Case "add"
                        blnCanAccess = CheckUserRight(htRight, eAccessType.Insert)
                        blnCanAdd = blnCanAccess
                    Case "update"
                        blnCanAccess = CheckUserRight(htRight, eAccessType.Update)
                        blnCanUpdate = blnCanAccess
                    Case "delete"
                        blnCanAccess = CheckUserRight(htRight, eAccessType.Delete)
                        blnCanDelete = blnCanAccess
                End Select
                '//to solve share button problem
                If blnCanAccess Then
                    While myEnumerator2.MoveNext

                        'CType(myEnumerator2.Current, System.Web.UI.WebControls.Button).Enabled = blnCanAccess
                        myEnumerator2.Current.Enabled = blnCanAccess

                    End While
                End If
            End While
        End Sub

        Public Function IsAuthentication() As Boolean
            Dim objDb As New EAD.DBCom
            Dim tDS As DataSet
            Dim strSQL, strField As String

            strSQL = "SELECT MM_MENU_ID, " &
            "CASE WHEN SUM(CNTview)=0 THEN 'N' ELSE 'Y' END  as sumView, " &
            "CASE WHEN SUM(CNTInsert)=0 THEN 'N' ELSE 'Y' END  as sumInsert, " &
            "CASE WHEN SUM(CNTUpdate)=0 THEN 'N' ELSE 'Y' END  as sumUpdate, " &
            "CASE WHEN SUM(CNTDelete)=0 THEN 'N' ELSE 'Y' END  as sumDelete " &
            "FROM(SELECT MM_MENU_ID, " &
            "CASE when UAR_allow_view='Y' then 1 else 0 end as cntView , " &
            "CASE when UAR_allow_insert='Y' then 1 else 0 end as cntInsert,  " &
            "CASE when UAR_allow_update='Y' then 1 else 0 end as cntUpdate, " &
            "CASE when UAR_allow_delete='Y' then 1 else 0 end as cntDelete " &
            "FROM MENU_MSTR M, USER_MSTR U,USER_ACCESS_RIGHT R ,USERS_USRGRP G " &
            "WHERE M.MM_GROUP='ehub' " &
            "AND U.UM_USER_ID=G.UU_USER_ID AND UM_COY_ID=UU_COY_ID " &
            "AND UU_USRGRP_ID=R.UAR_USRGRP_ID " &
            "AND MM_MENU_ID=UAR_MENU_ID  " &
            "AND UM_COY_ID='" & Session("CompanyId") & "' " &
            "AND G.uu_USER_ID='" & Session("UserId") & "' AND MM_MENU_ID=" & strPageId & ")a " &
            "GROUP BY MM_MENU_ID  " &
            "ORDER BY  MM_MENU_ID "
            tDS = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If IsDBNull(tDS.Tables(0).Rows(j).Item("sumView")) Then
                    If tDS.Tables(0).Rows(j).Item("sumInsert") = "N" Then
                        '//unauthorised access
                        '//if cannot view, cannot do add,update.. also
                        Dim vbs, strURL As String
                        'ConfigurationManager.AppSettings("SSLHttp")
                        If ConfigurationManager.AppSettings("SSLHttp") Then
                            strURL = "https://" & Request.ServerVariables("HTTP_Host") & "/"
                        Else
                            strURL = "http://" & Request.ServerVariables("HTTP_Host") & "/"
                        End If

                        strURL &= Request.ApplicationPath & "/Unauthorized.aspx"
                        vbs = vbs & "<script language=""vbs"">"
                        vbs = vbs & vbLf & "parent.location=""" & strURL & """"
                        vbs = vbs & "</script>"
                        Response.Write(vbs)
                        Exit Function
                    End If
                End If

                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("sumInsert")) Then
                    blnCanAdd = IIf(tDS.Tables(0).Rows(j).Item("sumInsert") = "Y", True, False)
                End If

                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("sumUpdate")) Then
                    blnCanUpdate = IIf(tDS.Tables(0).Rows(j).Item("sumUpdate") = "Y", True, False)
                End If

                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("sumDelete")) Then
                    blnCanDelete = IIf(tDS.Tables(0).Rows(j).Item("sumDelete") = "Y", True, False)
                End If
            Next



        End Function

        'Michelle - (12/10/2012)
        Public Function LogUserSreenAct(ByVal pIn As String)
            Dim lsSql As String
            'Michelle - (23/10/2012) Issue1755
            '           Dim objDB As New DBAccess.DBCom
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim ctx As System.Web.HttpContext = System.Web.HttpContext.Current

            lsSql = "INSERT INTO system_log(SL_COY_ID, SL_USER_ID, SL_USER_IP, SL_LOG_DATETIME, SL_URL, SL_SESSION_ID)Values('" &
            AgoraNEO.Common.Parse(ctx.Session("CompanyID")) & "','" &
            AgoraNEO.Common.Parse(ctx.Session("UserID")) & "','" &
            AgoraNEO.Common.Parse(ctx.Request.UserHostAddress) & "',NOW(), '" &
            AgoraNEO.Common.Parse(pIn) & "','" &
            AgoraNEO.Common.Parse(ctx.Session.SessionID) & "')"
            Try
                objDb.Execute(lsSql)
            Catch Err As Exception
                EventLog.WriteEntry(Err.Source & "--" & Left(lsSql, 150), "Error Occured in Function LogToDB.", EventLogEntryType.Error, 65535)
            Finally
                objDb = Nothing
            End Try
        End Function
#End Region
    End Class
End Namespace

