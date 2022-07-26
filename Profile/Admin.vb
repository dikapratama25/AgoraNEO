'2nd try
'aaaa
' Another Try
Imports System
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraNEO
Imports SSO.Component

Namespace AgoraNEO
    Public Class Admin
        Dim objDb As New EAD.DBCom
        Dim lsSql As String
        Public Function SalesArea(ByVal LocalSales As String, ByVal ExportSales As String)
            Dim strsqlchk, strsqladd, strsqlupdate As String
            Dim UserID, CoyID As String
            UserID = HttpContext.Current.Session("UserId")
            CoyID = HttpContext.Current.Session("CompanyId")

            strsqlchk = "SELECT CM_COY_ID FROM COMPANY_MISC WHERE CM_COY_ID='" & CoyID & "'"
            Dim chkDs As DataSet = objDb.FillDs(strsqlchk)

            If chkDs.Tables(0).Rows.Count > 0 Then
                strsqladd = "UPDATE COMPANY_MISC SET CM_LOCAL_SALES = '" & LocalSales & "', CM_EXPORT_SALES = '" & ExportSales &
                "', CM_MOD_BY = '" & UserID & "', CM_MOD_DT = " & Common.ConvertDate(Now) & " WHERE CM_COY_ID = '" & CoyID & "'"


                objDb.Execute(strsqladd)
            Else

                strsqlupdate = "INSERT INTO COMPANY_MISC(CM_COY_ID, CM_LOCAL_SALES, CM_EXPORT_SALES, CM_ENT_BY, CM_ENT_DT) VALUES ('" &
                CoyID & "','" & LocalSales & "','" & ExportSales & "','" & UserID & "'," & Common.ConvertDate(Now) & ")"

                objDb.Execute(strsqlupdate)

            End If




        End Function

        Public Function GetLocalSalesArea(ByVal CoyID As String) As String
            Dim sqlstr As String
            Dim localsales_value As String

            sqlstr = "SELECT CM_LOCAL_SALES from COMPANY_MISC where CM_COY_ID='" & CoyID & "' "
            Dim tDS As DataSet = objDb.FillDs(sqlstr)

            If tDS.Tables(0).Rows.Count > 0 Then
                localsales_value = tDS.Tables(0).Rows(0).Item("CM_LOCAL_SALES")
            End If

            Return localsales_value

        End Function
        Public Function GetExportSalesArea(ByVal CoyID As String) As String
            Dim sqlstr As String
            Dim exportsales_value As String

            sqlstr = "SELECT CM_EXPORT_SALES from COMPANY_MISC where CM_COY_ID='" & CoyID & "' "
            Dim tDS As DataSet = objDb.FillDs(sqlstr)

            If tDS.Tables(0).Rows.Count > 0 Then
                exportsales_value = tDS.Tables(0).Rows(0).Item("CM_EXPORT_SALES")
            End If

            Return exportsales_value

        End Function
        Public Function AddSalesTurnOver(ByVal CoyID As String, ByVal Year As String, ByVal Currency As String, ByVal Amount As Decimal) As Boolean
            Dim strsql, chkstrsql As String
            Dim UserID As String
            UserID = HttpContext.Current.Session("UserId")
            chkstrsql = "SELECT * FROM COMPANY_SALES WHERE CS_COY_ID ='" & CoyID & "' AND CS_YEAR = '" & Year & "'"
            Dim chkDS As DataSet = objDb.FillDs(chkstrsql)
            If chkDS.Tables(0).Rows.Count > 0 Then
                Return False
            Else
                strsql = "INSERT INTO COMPANY_SALES (CS_COY_ID, CS_YEAR, CS_CURRENCY_CODE, CS_AMOUNT, CS_ENT_BY, CS_ENT_DT) VALUES ('" &
                CoyID & "','" & Year & "','" & Currency &
                 "','" & Amount & "','" & UserID & "' ," & Common.ConvertDate(Now) & " )"

                objDb.Execute(strsql)
                Return True


            End If






        End Function
        Public Function delSalesTurnOver(ByVal CoyID As String, ByVal Year As String)
            Dim strsql As String
            Dim UserID As String
            UserID = HttpContext.Current.Session("UserId")
            strsql = "DELETE FROM COMPANY_SALES WHERE CS_COY_ID = '" & CoyID & "' AND CS_YEAR = '" & Year & "'"

            objDb.Execute(strsql)




        End Function
        Public Function ModifySalesTurnOver(ByVal CoyID As String, ByVal Year As String, ByVal Currency As String, ByVal Amount As Decimal)
            Dim strsql As String
            Dim UserID As String
            UserID = HttpContext.Current.Session("UserId")
            strsql = "UPDATE COMPANY_SALES SET CS_YEAR ='" & Year & "', CS_CURRENCY_CODE ='" & Currency &
            "', CS_AMOUNT ='" & Amount & "', CS_MOD_BY ='" & UserID & "', CS_MOD_DT =" & Common.ConvertDate(Now) & " WHERE CS_COY_ID = '" & CoyID & "' AND CS_YEAR = '" & Year & "'"

            objDb.Execute(strsql)




        End Function


        Public Function getSalesInfoList(ByVal CoyID As String) As DataSet
            Dim strGet As String
            Dim dtSalesInfo As DataSet
            strGet = "SELECT CS_YEAR, CS_CURRENCY_CODE, CS_AMOUNT, CS_ENT_DT FROM COMPANY_SALES WHERE CS_COY_ID = '" & Common.Parse(CoyID) & "'"
            dtSalesInfo = objDb.FillDs(strGet)

            Return dtSalesInfo


        End Function

        Public Function SetNULL_consol()
            Dim strSql As String
            strSql = "UPDATE APPROVAL_GRP_MSTR set AGM_CONSOLIDATOR=NULL WHERE AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "'"
            objDb.Execute(strSql)
        End Function


        Public Function PopulateAddress() As DataSet
            Dim strSql As String
            Dim dsAddr As DataSet
            strSql = "Select AM.AM_Coy_ID, AM.AM_Addr_Code, " &
             objDb.Concat(" ", "", "AM.AM_Addr_Line1", "AM.AM_Addr_Line2", "AM.AM_Addr_Line3") & " as [Billing Address]" &
             " from Address_Mstr AM, COMPANY_MSTR CM" &
             " where AM.AM_Coy_ID = CM.CM_Coy_ID " &
             " and  AM.AM_Coy_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
            " order by  AM.AM_Addr_Code "
            dsAddr = objDb.FillDs(strSql)
            PopulateAddress = dsAddr
        End Function

        Public Function PopulateAddressPR(ByVal sCoyID As String, ByVal sAddrCode As String, ByVal sPR_No As String) As DataSet
            Dim strSql As String
            Dim dsAddr As DataSet

            'PRM_B_ADDR_CODE, PRM_B_ADDR_LINE1, PRM_B_ADDR_LINE2, PRM_B_LINE_ADDR_LINE3, 
            'PRM_B_POSTCODE, PRM_B_STATE, PRM_B_CITY, PRM_B_COUNTRY

            strSql = "SELECT PRM_B_ADDR_CODE AS AM_ADDR_CODE, PRM_B_ADDR_LINE1 AS AM_ADDR_LINE1, PRM_B_ADDR_LINE2 AS AM_ADDR_LINE2, PRM_B_ADDR_LINE3 AS AM_ADDR_LINE3, PRM_B_POSTCODE AS AM_POSTCODE, PRM_B_CITY AS AM_CITY, " &
                "PRM_B_STATE AS AM_STATE, PRM_B_COUNTRY AS AM_COUNTRY,B.CODE_DESC AS STATE, C.CODE_DESC AS COUNTRY FROM PR_MSTR " &
                "LEFT JOIN CODE_MSTR AS B ON PRM_B_STATE = B.CODE_ABBR AND B.CODE_CATEGORY = 'S' AND B.CODE_VALUE = PRM_B_COUNTRY " &
                "LEFT JOIN CODE_MSTR AS C ON PRM_B_COUNTRY = C.CODE_ABBR AND C.CODE_CATEGORY = 'CT' " &
                "WHERE PRM_COY_ID = '" & sCoyID & "' AND PRM_B_ADDR_CODE = '" & sAddrCode & "' AND PRM_PR_NO = '" & sPR_No & "' "

            dsAddr = objDb.FillDs(strSql)
            PopulateAddressPR = dsAddr

        End Function

        'Yik Foong
        '22 Oct 2009
        'Get GL Codes and GL Description
        'Parameters :
        '	1. strProdCode : The product code
        '	2. strItemCode : The item code
        '
        'Return :
        '	A dataset the contain 2 columns' table
        '			
        'Columns :
        '	"GL Code" : Contain the GL Code
        '	"DESCRIPTION" : Contain (<GL Code>) <GL Description>, if <GL Description> is null,
        '								it only contian (<GL Code>)
        Public Function PopulateGLCode(ByVal strProdCode As String, ByVal strItemCode As String) As DataSet    'Michelle (eBiz/303)
            Dim strSql, strVal As String
            Dim dsGLCode As DataSet
            'Get GL_CODE from COMPANY_B_ITEM_CODE, if no record found, take from COMPANY_B_GL_CODE
            'strSql = "Select CBC_B_GL_CODE as [GL Code] " & _
            ' " from COMPANY_B_ITEM_CODE " & _
            ' "where CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " & _
            '" and CBC_PRODUCT_CODE = '" & strProdCode & "' and CBC_B_ITEM_CODE = '" & strItemCode & "' "
            strSql = "Select CBC_B_GL_CODE as 'GL Code' " &
             " from COMPANY_B_ITEM_CODE " &
             "where CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
            " and CBC_PRODUCT_CODE = '" & strProdCode & "' and CBC_B_ITEM_CODE = '" & strItemCode & "' "

            strVal = ""
            strVal = objDb.GetVal(strSql)

            If strVal <> "" Then
                strSql = "Select CBC_B_GL_CODE as 'GL Code' , " &
                objDb.Concat("", "", objDb.Concat("(", ")", "CBC_B_GL_CODE"), "(SELECT case when count(*) > 0 then (select a.CBG_B_GL_DESC from COMPANY_B_GL_CODE a where a.CBG_B_GL_CODE =  CBC_B_GL_CODE and a.CBG_B_COY_ID = CBC_B_COY_ID)  else ' ' end FROM COMPANY_B_GL_CODE WHERE COMPANY_B_GL_CODE.CBG_B_GL_CODE = CBC_B_GL_CODE AND COMPANY_B_GL_CODE.CBG_B_COY_ID = CBC_B_COY_ID)") & " AS DESCRIPTION " &
                "from COMPANY_B_ITEM_CODE " &
                "where CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") &
                "' and CBC_PRODUCT_CODE = '" & strProdCode & "' " &
                "and CBC_B_ITEM_CODE = '" & strItemCode & "' "
                dsGLCode = objDb.FillDs(strSql)
            Else
                strSql = "Select CBG_B_GL_CODE as 'GL Code' ,  " &
                    objDb.Concat("", "", objDb.Concat("(", ")", "CBG_B_GL_CODE"), "CBG_B_GL_DESC") & " as DESCRIPTION " &
                 " from COMPANY_B_GL_CODE where CBG_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
                " order by  CBG_B_GL_CODE "

                dsGLCode = objDb.FillDs(strSql)
            End If

            PopulateGLCode = dsGLCode
        End Function

        Public Function PopulateGLCodeMstr(ByVal strProdCode As String, ByVal strGLCode As String, Optional ByVal strFor As String = "") As DataSet
            Dim strSql As String
            Dim dsGLCode As DataSet

            'Jules 2018.11.05 - Swapped GL Code display: "GL Description (GL Code) and order by description.
            If Trim(strProdCode) <> "" And strFor = "B" Then
                strSql = "Select CBG_B_GL_CODE as 'GL Code' ,  " &
                    objDb.Concat("", "", "CBG_B_GL_DESC", objDb.Concat(" (", ")", "CBG_B_GL_CODE")) & " as DESCRIPTION " &
                 " from COMPANY_B_GL_CODE where CBG_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND CBG_B_GL_CODE = '" & strGLCode & "' " &
                " order by DESCRIPTION " 'order by CBG_B_GL_CODE

                dsGLCode = objDb.FillDs(strSql)
            Else
                strSql = "Select CBG_B_GL_CODE as 'GL Code' ,  " &
                    objDb.Concat("", "", "CBG_B_GL_DESC", objDb.Concat(" (", ")", "CBG_B_GL_CODE")) & " as DESCRIPTION " &
                 " from COMPANY_B_GL_CODE where CBG_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
                " order by DESCRIPTION " 'order by CBG_B_GL_CODE

                dsGLCode = objDb.FillDs(strSql)
            End If

            PopulateGLCodeMstr = dsGLCode
        End Function

        Public Function PopulateCategoryCodeMstr(ByVal strProdCode As String, ByVal strCatCode As String, Optional ByVal strFor As String = "") As DataSet      'Michelle (eBiz/303)
            Dim strSql As String
            Dim dsCategoryCode As DataSet

            If Trim(strProdCode) <> "" And strFor = "B" Then
                strSql = "Select CBC_B_CATEGORY_CODE as 'Category Code'" &
                 " from COMPANY_B_CATEGORY_CODE where CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND CBC_B_CATEGORY_CODE = '" & strCatCode & "'" &
                " order by  CBC_B_CATEGORY_CODE "
                dsCategoryCode = objDb.FillDs(strSql)
            Else
                strSql = "Select CBC_B_CATEGORY_CODE as 'Category Code'" &
                 " from COMPANY_B_CATEGORY_CODE where CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
                " order by  CBC_B_CATEGORY_CODE "
                dsCategoryCode = objDb.FillDs(strSql)
            End If

            Dim table As DataTable = dsCategoryCode.Tables(0)
            For Each row As DataRow In table.Rows
                If CStr(row("Category Code")).Equals("") Then
                    row("Category Code") = Nothing
                End If
            Next

            PopulateCategoryCodeMstr = dsCategoryCode
        End Function

        Public Function PopulateCategoryCode(ByVal strProdCode As String, ByVal strItemCode As String) As DataSet      'Michelle (eBiz/303)
            Dim strSql, strVal As String
            Dim dsCategoryCode As DataSet
            'Get Category_CODE from COMPANY_B_ITEM_CODE, if no record found, take from COMPANY_B_CATEGORY_CODE
            strSql = "Select CBC_B_CATEGORY_CODE as 'Category Code'" &
             " from COMPANY_B_ITEM_CODE where CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
            " and CBC_PRODUCT_CODE = '" & strProdCode & "' and CBC_B_ITEM_CODE = '" & strItemCode & "' "

            strVal = ""
            strVal = objDb.GetVal(strSql)

            'If Not strVal Is Nothing Then
            '    If strVal <> "" Then
            '        dsCategoryCode = objDb.FillDs(strSql)

            '    Else
            '        strSql = "Select CBC_B_CATEGORY_CODE as [Category Code]" & _
            '         " from COMPANY_B_CATEGORY_CODE where CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " & _
            '        " order by  CBC_B_CATEGORY_CODE "
            '        dsCategoryCode = objDb.FillDs(strSql)
            '    End If
            'End If

            If strVal <> "" Then
                dsCategoryCode = objDb.FillDs(strSql)
            Else
                strSql = "Select CBC_B_CATEGORY_CODE as 'Category Code'" &
                 " from COMPANY_B_CATEGORY_CODE where CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
                " order by  CBC_B_CATEGORY_CODE "
                dsCategoryCode = objDb.FillDs(strSql)
            End If

            Dim table As DataTable = dsCategoryCode.Tables(0)
            For Each row As DataRow In table.Rows
                If CStr(row("Category Code")).Equals("") Then
                    row("Category Code") = Nothing
                End If
            Next

            PopulateCategoryCode = dsCategoryCode
        End Function

        Public Function user_Default_Add_ByDefault(ByVal strType As String) As String

            Dim user_default As String
            Dim sqlstr, strsql1, strsql2 As String
            Dim objUser As New Users
            Dim objUserDetails As New User
            Dim objDb As New EAD.DBCom

            strsql2 = ""
            objUserDetails = objUser.GetUserDetails(HttpContext.Current.Session("UserId"), HttpContext.Current.Session("CompanyId"))
            If objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Buyer) Or objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Purchasing_Officer) Or objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Purchasing_Manager) Then
                strsql1 = "SELECT '*' FROM USERS_ADDR WHERE UA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql1 &= "AND UA_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
                strsql1 &= "AND UA_ADDR_TYPE = '" & strType & "' "
                strsql1 &= "AND UA_VIEW_OPTION = 1"
                If objDb.Exist(strsql1) = 0 Then
                    strsql2 = "SELECT UA_ADDR_CODE AS UDA_ADDR_CODE FROM USERS_ADDR "
                    strsql2 &= "WHERE UA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql2 &= "AND UA_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
                    strsql2 &= "AND UA_ADDR_TYPE = '" & strType & "' "
                    strsql2 &= "AND UA_VIEW_OPTION = 0 "
                End If
            End If

            sqlstr = "SELECT UDA_ADDR_CODE from USERS_DEFAULT_ADDR where UDA_ADDR_TYPE='" & strType & "' "
            sqlstr &= "AND UDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            sqlstr &= "AND UDA_USER_ID = '" & HttpContext.Current.Session("UserId") & "'"

            If strsql2 <> "" Then
                sqlstr &= "AND UDA_ADDR_CODE IN ( " & strsql2 & " ) "
            End If

            Dim tDS As DataSet
            If objDb.Exist(sqlstr) = 0 Then

                If strsql2 <> "" Then
                    strsql2 &= " LIMIT 1 "

                Else
                    strsql2 = " SELECT AM_ADDR_CODE AS UDA_ADDR_CODE FROM ADDRESS_MSTR WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND AM_ADDR_TYPE = 'D' ORDER BY AM_ADDR_CODE LIMIT 1 "
                End If
                tDS = objDb.FillDs(strsql2)
            Else
                tDS = objDb.FillDs(sqlstr)
            End If

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                user_default = tDS.Tables(0).Rows(j).Item("UDA_ADDR_CODE")
            Next

            user_Default_Add_ByDefault = user_default
        End Function

        Public Function user_Default_Add(ByVal userID As String, ByVal companyID As String, ByVal strType As String) As String

            Dim user_default As String = String.Empty
            Dim sqlstr As String

            sqlstr = "SELECT UDA_ADDR_CODE from USERS_DEFAULT_ADDR where UDA_ADDR_TYPE='" & strType & "' "
            sqlstr &= "AND UDA_COY_ID = '" & companyID & "' "
            sqlstr &= "AND UDA_USER_ID = '" & userID & "'"
            Dim tDS As DataSet = objDb.FillDs(sqlstr)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                user_default = tDS.Tables(0).Rows(j).Item("UDA_ADDR_CODE")
            Next

            user_Default_Add = If(Not String.IsNullOrEmpty(user_default), user_default, String.Empty)
        End Function

        Public Function CustomAddr(ByVal strType As String, Optional ByVal udcType As String = "") As String
            Dim customer_field As String
            Dim sqlstr As String
            Dim custom_value As String

            sqlstr = "SELECT UDC_FIELD_VALUE from USERS_DEFAULT_CUSTOMFIELDS where UDC_FIELD_NO='" & strType & "' " &
             "AND UDC_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
             "AND UDC_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
            If udcType <> "" Then
                sqlstr &= "AND UDC_TYPE = '" & udcType & "' "
            End If
            Dim tDS As DataSet = objDb.FillDs(sqlstr)

            If tDS.Tables(0).Rows.Count > 0 Then
                custom_value = tDS.Tables(0).Rows(0).Item("UDC_FIELD_VALUE")
            End If

            CustomAddr = custom_value
        End Function

        Public Function Populate_customField(ByVal companyID As String, ByVal strType As String, ByVal strvalue As String, Optional ByVal strModule As String = "") As DataSet
            Dim strSql As String
            Dim dsAddr As DataSet
            strSql = "SELECT CF_FIELD_INDEX, CF_FIELD_VALUE, REPLACE(CF_FIELD_VALUE,' ','_') AS FIELDVALUE FROM CUSTOM_FIELDS WHERE CF_FIELD_NO ='" & strType & "' " &
               "AND CF_COY_ID = '" & companyID & "' "
            'If strvalue <> "" Then
            '    strSql &= "AND CF_FIELD_VALUE LIKE '%" & strvalue & "%' "
            'End If

            If strvalue <> "" Then
                strSql &= "AND CF_FIELD_VALUE" & Common.ParseSQL(strvalue)
            End If

            If strModule <> "" Then
                strSql &= " AND CF_MODULE" & Common.ParseSQL(strModule)
            Else
                strSql &= " AND CF_MODULE='PR'"
            End If

            strSql = strSql & " ORDER BY CF_FIELD_VALUE"

            dsAddr = objDb.FillDs(strSql)
            Populate_customField = dsAddr
        End Function

        Public Function Populate_customFieldPR(ByVal strType As String, ByVal strvalue As String, Optional ByVal strModule As String = "", Optional ByVal strPRIndex As String = "") As DataSet
            Dim strSql As String
            Dim dsAddr As DataSet
            'strSql = "SELECT CF_FIELD_INDEX, CF_FIELD_VALUE, REPLACE(CF_FIELD_VALUE,' ','_') AS FIELDVALUE " & _
            '         "FROM CUSTOM_FIELDS WHERE CF_FIELD_NO ='" & strType & "' " & _
            '         "AND CF_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            strSql = " SELECT PCD_PR_INDEX AS CF_FIELD_INDEX, PCD_FIELD_VALUE AS CF_FIELD_VALUE, " &
                     " REPLACE(PCD_FIELD_VALUE,' ','_') AS FIELDVALUE " &
                     " FROM PR_CUSTOM_FIELD_DETAILS " &
                     " WHERE PCD_PR_INDEX = '" & strPRIndex & "' "

            'If strvalue <> "" Then
            '    strSql &= "AND CF_FIELD_VALUE LIKE '%" & strvalue & "%' "
            'End If

            If strvalue <> "" Then
                strSql &= "AND PCD_FIELD_VALUE" & Common.ParseSQL(strvalue)
            End If

            If strModule <> "" Then
                strSql &= " AND PCD_TYPE" & Common.ParseSQL(strModule)
            Else
                strSql &= " AND PCD_TYPE='PR'"
            End If

            strSql = strSql & " ORDER BY PCD_FIELD_VALUE"

            dsAddr = objDb.FillDs(strSql)
            Populate_customFieldPR = dsAddr
        End Function

        Public Function Populate_customFieldPR2(ByVal strType As String, ByVal strvalue As String, Optional ByVal strModule As String = "", Optional ByVal strPRIndex As String = "") As DataSet
            Dim strSql As String
            Dim dsAddr As DataSet
            'strSql = "SELECT CF_FIELD_INDEX, CF_FIELD_VALUE, REPLACE(CF_FIELD_VALUE,' ','_') AS FIELDVALUE " & _
            '         "FROM CUSTOM_FIELDS WHERE CF_FIELD_NO ='" & strType & "' " & _
            '         "AND CF_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            strSql = " SELECT PCD_PR_INDEX AS CF_FIELD_INDEX, PCD_FIELD_VALUE AS CF_FIELD_VALUE, " &
                     " REPLACE(PCD_FIELD_VALUE,' ','_') AS FIELDVALUE " &
                     " FROM PR_CUSTOM_FIELD_DETAILS " &
                     " WHERE PCD_PR_INDEX IN (" & strPRIndex & ") "

            'If strvalue <> "" Then
            '    strSql &= "AND CF_FIELD_VALUE LIKE '%" & strvalue & "%' "
            'End If

            If strvalue <> "" Then
                strSql &= "AND PCD_FIELD_VALUE" & Common.ParseSQL(strvalue)
            End If

            If strModule <> "" Then
                strSql &= " AND PCD_TYPE" & Common.ParseSQL(strModule)
            Else
                strSql &= " AND PCD_TYPE='PR'"
            End If

            strSql = strSql & " ORDER BY PCD_FIELD_VALUE"

            dsAddr = objDb.FillDs(strSql)
            Populate_customFieldPR2 = dsAddr
        End Function

        Public Function Populate_ProductCode(ByVal strVendor As String, ByVal strID As String, ByVal strDesc As String) As DataSet
            Dim strSql As String
            Dim dsVendor As DataSet

            'Michelle (16/6/2010) - To include Tax Code
            'strSql = "SELECT PM_PRODUCT_CODE, PM_PRODUCT_DESC, CBC_B_ITEM_CODE, CBC_B_CATEGORY_CODE, CBC_B_GL_CODE  "
            strSql = "SELECT PM_PRODUCT_CODE, PM_PRODUCT_DESC, CBC_B_ITEM_CODE, CBC_B_CATEGORY_CODE, CBC_B_GL_CODE, CBC_B_TAX_CODE  "
            strSql &= "FROM PRODUCT_MSTR LEFT JOIN COMPANY_B_ITEM_CODE ON CBC_PRODUCT_CODE = PM_PRODUCT_CODE "
            strSql &= "AND CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSql &= "WHERE PM_S_COY_ID ='" & strVendor & "' "
            strSql &= "AND PM_DELETED <> 'Y' "

            If strID <> "" Then
                strSql &= "AND PM_PRODUCT_CODE" & Common.ParseSQL(strID)
            End If

            If strDesc <> "" Then
                strSql &= "AND PM_PRODUCT_DESC" & Common.ParseSQL(strDesc)
            End If

            'If strState <> "" Then
            '    strSql &= "AND AM_STATE = '" & strState & "'"
            'End If
            dsVendor = objDb.FillDs(strSql)
            Populate_ProductCode = dsVendor

        End Function

        Public Class PopulateAddrResult
            Public strSql As String
            Public dsAddr As DataSet
        End Class

        Public Function PopulateAddr(ByVal userID As String, ByVal companyID As String, ByVal strType As String, ByVal strCode As String, ByVal strCity As String,
                ByVal strState As String, Optional ByVal blnSortCode As Boolean = False, Optional ByVal getStrSQL As Boolean = False) As PopulateAddrResult
            Dim strSql, strsql1, strsql2 As String
            Dim dsAddr As DataSet
            Dim objUser As New Users
            Dim objUserDetails As New User
            Dim objDb As New EAD.DBCom

            strsql2 = ""
            objUserDetails = objUser.GetUserDetails(userID, companyID)
            If objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Buyer) Or objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Purchasing_Officer) Or objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Purchasing_Manager) Then
                strsql1 = "SELECT '*' FROM USERS_ADDR WHERE UA_COY_ID = '" & companyID & "' "
                strsql1 &= "AND UA_USER_ID = '" & userID & "' "
                strsql1 &= "AND UA_ADDR_TYPE = '" & strType & "' "
                strsql1 &= "AND UA_VIEW_OPTION = 1"
                If objDb.Exist(strsql1) = 0 Then
                    strsql2 = "SELECT UA_ADDR_CODE FROM USERS_ADDR "
                    strsql2 &= "WHERE UA_COY_ID = '" & companyID & "' "
                    strsql2 &= "AND UA_USER_ID = '" & userID & "' "
                    strsql2 &= "AND UA_ADDR_TYPE = '" & strType & "' "
                    strsql2 &= "AND UA_VIEW_OPTION = 0 "
                End If
            End If

            '//by Moo(04/10/2005)
            '//Take out the "," between ADDR_LINE1 and ADDR_LINE2,ADDR_LINE2 and ADDR_LINE3
            '//because user may enter "," at the end of ADDR_LINE1,ADDR_LINE2,ADDR_LINE3
            '//FullAddress --> address + ' - ' + addresscode (required at RaisePR.aspx only)
            strSql = "SELECT AM_ADDR_INDEX, AM_COY_ID, AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, "
            'Michelle (27/9/2010) - To cater for MYSQL syntax
            'strSql &= "(AM_ADDR_LINE1 + ' ' + CASE WHEN AM_ADDR_LINE2 IS NOT NULL THEN AM_ADDR_LINE2 ELSE '' END + ' ' + CASE WHEN AM_ADDR_LINE3 IS NOT NULL THEN AM_ADDR_LINE3 ELSE '' END) AS Address, AM_CITY, AM_STATE, "
            strSql &= "(" & objDb.Concat(" ", "", "AM_ADDR_LINE1", "CASE WHEN AM_ADDR_LINE2 IS NOT NULL THEN AM_ADDR_LINE2 ELSE '' END", "CASE WHEN AM_ADDR_LINE3 IS NOT NULL THEN AM_ADDR_LINE3 ELSE '' END") & ")"
            strSql &= " AS Address, AM_CITY, AM_STATE, "
            strSql &= "AM_POSTCODE, AM_COUNTRY, B.CODE_DESC AS STATE, C.CODE_DESC AS COUNTRY, "
            'strSql &= "(AM_ADDR_LINE1 + ', ' + "
            strSql &= "(" & objDb.MultiConcat("AM_ADDR_LINE1", ", ", "CASE WHEN AM_ADDR_LINE2 IS NOT NULL AND AM_ADDR_LINE2 <> '' THEN AM_ADDR_LINE2 ELSE '' END", " ", "CASE WHEN AM_ADDR_LINE3 IS NOT NULL AND AM_ADDR_LINE3 <> '' THEN AM_ADDR_LINE3 ELSE '' END", " ", "AM_POSTCODE", " ", "AM_CITY", ", ", "CASE WHEN B.CODE_DESC IS NOT NULL AND B.CODE_DESC <> '' THEN B.CODE_DESC ELSE '' END", ", ", "CASE WHEN C.CODE_DESC IS NOT NULL AND C.CODE_DESC <> '' THEN C.CODE_DESC ELSE '' END ", " - ", "AM_ADDR_CODE")
            '         strSql &= "CASE WHEN B.CODE_DESC IS NOT NULL AND B.CODE_DESC <> '' THEN B.CODE_DESC + ', ' ELSE '' END + "
            'strSql &= "CASE WHEN C.CODE_DESC IS NOT NULL AND C.CODE_DESC <> '' THEN C.CODE_DESC + '' ELSE '' END "
            strSql &= ") AS FullAddress "
            strSql &= "FROM ADDRESS_MSTR "
            strSql &= "LEFT JOIN CODE_MSTR AS B ON AM_STATE = B.CODE_ABBR AND B.CODE_CATEGORY = 'S' AND B.CODE_VALUE = AM_COUNTRY "
            strSql &= "LEFT JOIN CODE_MSTR AS C ON AM_COUNTRY = C.CODE_ABBR AND C.CODE_CATEGORY = 'CT' "
            strSql &= "WHERE AM_COY_ID = '" & companyID & "' "
            strSql &= "AND AM_ADDR_TYPE = '" & strType & "' "

            If strCode <> "" Then
                strSql &= "AND AM_ADDR_CODE" & Common.ParseSQL(strCode) & " "
            End If

            If strCity <> "" Then
                strSql &= "AND AM_CITY" & Common.ParseSQL(strCity) & " "
            End If

            If strState <> "" Then
                strSql &= "AND AM_STATE = '" & strState & "' "
            End If

            If strsql2 <> "" Then
                strSql &= "AND AM_ADDR_CODE IN ( " & strsql2 & " ) "
            End If

            If blnSortCode Then
                strSql &= "ORDER BY AM_ADDR_CODE, AM_ADDR_LINE1"
            Else
                strSql &= "ORDER BY AM_ADDR_LINE1"
            End If

            Dim result As PopulateAddrResult = New PopulateAddrResult
            If Not getStrSQL Then
                dsAddr = objDb.FillDs(strSql)
            Else
                dsAddr = Nothing
            End If
            result.strSql = strSql
            result.dsAddr = dsAddr
            PopulateAddr = result
        End Function

        'Yik Foong
        'Change : modified from Function PopulateAddr( String, String, String, String, Boolean )
        '		Added search by address field
        Public Function PopulateAddr(ByVal strType As String, ByVal strCode As String, ByVal strAddress As String, ByVal strCity As String,
         ByVal strState As String, Optional ByVal blnSortCode As Boolean = False, Optional ByVal strCountry As String = "") As DataSet
            Dim strSql, strsql1, strsql2 As String
            Dim dsAddr As DataSet
            Dim objUser As New Users
            Dim objUserDetails As New User
            Dim strTemp As String

            strsql2 = ""
            objUserDetails = objUser.GetUserDetails(HttpContext.Current.Session("UserId"), HttpContext.Current.Session("CompanyId"))
            If objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Buyer) Or objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Purchasing_Officer) Or objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Purchasing_Manager) Then
                strsql1 = "SELECT '*' FROM USERS_ADDR WHERE UA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql1 &= "AND UA_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
                strsql1 &= "AND UA_ADDR_TYPE = '" & strType & "' "
                strsql1 &= "AND UA_VIEW_OPTION = 1"
                If objDb.Exist(strsql1) = 0 Then
                    strsql2 = "SELECT UA_ADDR_CODE FROM USERS_ADDR "
                    strsql2 &= "WHERE UA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql2 &= "AND UA_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
                    strsql2 &= "AND UA_ADDR_TYPE = '" & strType & "' "
                    strsql2 &= "AND UA_VIEW_OPTION = 0 "
                End If
            End If

            '//by Moo(04/10/2005)
            '//Take out the "," between ADDR_LINE1 and ADDR_LINE2,ADDR_LINE2 and ADDR_LINE3
            '//because user may enter "," at the end of ADDR_LINE1,ADDR_LINE2,ADDR_LINE3
            '//FullAddress --> address + ' - ' + addresscode (required at RaisePR.aspx only)
            'Michelle (27/9/2010) - To cater for MYSQL syntax
            strSql = "SELECT AM_ADDR_INDEX, AM_COY_ID, AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, "
            'strSql &= "(AM_ADDR_LINE1 + ' ' + CASE WHEN AM_ADDR_LINE2 IS NOT NULL THEN AM_ADDR_LINE2 ELSE '' END + ' ' + CASE WHEN AM_ADDR_LINE3 IS NOT NULL THEN AM_ADDR_LINE3 ELSE '' END) AS Address, "
            'strSql &= "AM_CITY, AM_STATE, "
            'strSql &= "AM_POSTCODE, AM_COUNTRY, B.CODE_DESC AS STATE, C.CODE_DESC AS COUNTRY, "
            'strSql &= "(AM_ADDR_LINE1 + ', ' + "
            'strSql &= "CASE WHEN AM_ADDR_LINE2 IS NOT NULL AND AM_ADDR_LINE2 <> '' THEN AM_ADDR_LINE2 + ' ' ELSE '' END + "
            'strSql &= "CASE WHEN AM_ADDR_LINE3 IS NOT NULL AND AM_ADDR_LINE3 <> '' THEN AM_ADDR_LINE3 + ' ' ELSE '' END + "
            'strSql &= "+ AM_POSTCODE + ' ' + AM_CITY + ', ' + "
            'strSql &= "CASE WHEN B.CODE_DESC IS NOT NULL AND B.CODE_DESC <> '' THEN B.CODE_DESC + ', ' ELSE '' END + "
            'strSql &= "CASE WHEN C.CODE_DESC IS NOT NULL AND C.CODE_DESC <> '' THEN C.CODE_DESC + '' ELSE '' END "
            strSql &= "(" & objDb.Concat(" ", "", "AM_ADDR_LINE1", "CASE WHEN AM_ADDR_LINE2 IS NOT NULL THEN AM_ADDR_LINE2 ELSE '' END", "CASE WHEN AM_ADDR_LINE3 IS NOT NULL THEN AM_ADDR_LINE3 ELSE '' END") & ")"
            strSql &= " AS Address, AM_CITY, AM_STATE, "
            strSql &= "AM_POSTCODE, AM_COUNTRY, B.CODE_DESC AS STATE, C.CODE_DESC AS COUNTRY, "
            strSql &= "(" & objDb.MultiConcat("AM_ADDR_LINE1", ", ", "CASE WHEN AM_ADDR_LINE2 IS NOT NULL AND AM_ADDR_LINE2 <> '' THEN AM_ADDR_LINE2 ELSE '' END", " ", "CASE WHEN AM_ADDR_LINE3 IS NOT NULL AND AM_ADDR_LINE3 <> '' THEN AM_ADDR_LINE3 ELSE '' END", " ", "AM_POSTCODE", " ", "AM_CITY", ", ", "CASE WHEN B.CODE_DESC IS NOT NULL AND B.CODE_DESC <> '' THEN B.CODE_DESC ELSE '' END", ", ", "CASE WHEN C.CODE_DESC IS NOT NULL AND C.CODE_DESC <> '' THEN C.CODE_DESC ELSE '' END ", " - ", "AM_ADDR_CODE")
            strSql &= ") + ' - ' + AM_ADDR_CODE AS FullAddress "
            strSql &= "FROM ADDRESS_MSTR "
            strSql &= "LEFT JOIN CODE_MSTR AS B ON AM_STATE = B.CODE_ABBR AND B.CODE_CATEGORY = 'S' AND B.CODE_VALUE = AM_COUNTRY "
            strSql &= "LEFT JOIN CODE_MSTR AS C ON AM_COUNTRY = C.CODE_ABBR AND C.CODE_CATEGORY = 'CT' "
            strSql &= "WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSql &= "AND AM_ADDR_TYPE = '" & strType & "' "

            If strCode <> "" Then
                strTemp = Common.BuildWildCard(strCode)
                strSql &= "AND AM_ADDR_CODE" & Common.ParseSQL(strTemp) & " "
            End If

            If strAddress <> "" Then
                strTemp = Common.BuildWildCard(strAddress)
                Dim addressFilter As String = Common.ParseSQL(strTemp)
                strSql &= "AND (AM_ADDR_LINE1 " & addressFilter & " OR "
                strSql &= "AM_ADDR_LINE2 " & addressFilter & " OR "
                strSql &= "AM_ADDR_LINE3 " & addressFilter & " ) "
            End If

            If strCity <> "" Then
                strTemp = Common.BuildWildCard(strCity)
                strSql &= "AND AM_CITY" & Common.ParseSQL(strTemp) & " "
            End If

            If strState <> "" Then
                strSql &= "AND AM_STATE = '" & strState & "' "
            End If

            If strCountry <> "" Then
                strSql &= "AND AM_COUNTRY = '" & strCountry & "' "
            End If

            'Zulham 06122018
            'If strsql2 <> "" Then
            '    strSql &= "AND AM_ADDR_CODE IN ( " & strsql2 & " ) "
            'End If

            If blnSortCode Then
                strSql &= "ORDER BY AM_ADDR_CODE, AM_ADDR_LINE1"
            Else
                strSql &= "ORDER BY AM_ADDR_LINE1"
            End If

            dsAddr = objDb.FillDs(strSql)
            PopulateAddr = dsAddr
        End Function

        Public Function GetSpecificAddr(ByVal sCoyID As String, ByVal sAddrCode As String, ByVal sAddrType As String) As String
            Dim sAddr As String
            Dim tSQL As String = "SELECT AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, " &
            "AM_STATE, AM_COUNTRY,B.CODE_DESC AS STATE, C.CODE_DESC AS COUNTRY FROM address_mstr " &
            "LEFT JOIN CODE_MSTR AS B ON AM_STATE = B.CODE_ABBR AND B.CODE_CATEGORY = 'S' AND B.CODE_VALUE = AM_COUNTRY " &
            "LEFT JOIN CODE_MSTR AS C ON AM_COUNTRY = C.CODE_ABBR AND C.CODE_CATEGORY = 'CT' " &
            "WHERE AM_COY_ID = '" & sCoyID & "' AND AM_ADDR_CODE = '" & sAddrCode & "' AND AM_ADDR_TYPE = '" & sAddrType & "'"
            Dim tDS As DataSet = objDb.FillDs(tSQL)

            If tDS.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE1")) AndAlso tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE1") <> "" Then
                    sAddr = sAddr & " " & tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE1")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE2")) AndAlso tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE2") <> "" Then
                    sAddr = sAddr & ", " & tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE2")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE3")) AndAlso tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE3") <> "" Then
                    sAddr = sAddr & ", " & tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE3")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_POSTCODE")) Then
                    sAddr = sAddr & ", " & tDS.Tables(0).Rows(0).Item("AM_POSTCODE")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_CITY")) AndAlso tDS.Tables(0).Rows(0).Item("AM_CITY") <> "" Then
                    sAddr = sAddr & " " & tDS.Tables(0).Rows(0).Item("AM_CITY")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_STATE")) Then
                    sAddr = sAddr & " " & tDS.Tables(0).Rows(0).Item("AM_STATE")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_COUNTRY")) Then
                    sAddr = sAddr & " " & tDS.Tables(0).Rows(0).Item("AM_COUNTRY")
                End If
            End If
            GetSpecificAddr = sAddr
        End Function

        Public Function GetSpecificAddrPR(ByVal sCoyID As String, ByVal sAddrCode As String, ByVal sAddrType As String, ByVal sPRLineIndex As String) As String
            Dim sAddr As String
            Dim tDS As DataSet
            'PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1, PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, 
            'PRD_D_POSTCODE, PRD_D_CITY, PRD_D_STATE, PRD_D_COUNTRY

            Dim tSQL As String = "SELECT PRD_D_ADDR_LINE1 AS AM_ADDR_LINE1, PRD_D_ADDR_LINE2 AS AM_ADDR_LINE2, PRD_D_ADDR_LINE3 AS AM_ADDR_LINE3, PRD_D_POSTCODE AS AM_POSTCODE, PRD_D_CITY AS AM_CITY, " &
                "PRD_D_STATE AS AM_STATE, PRD_D_COUNTRY AS AM_COUNTRY,B.CODE_DESC AS STATE, C.CODE_DESC AS COUNTRY FROM PR_DETAILS " &
                "LEFT JOIN CODE_MSTR AS B ON PRD_D_STATE = B.CODE_ABBR AND B.CODE_CATEGORY = 'S' AND B.CODE_VALUE = PRD_D_COUNTRY " &
                "LEFT JOIN CODE_MSTR AS C ON PRD_D_COUNTRY = C.CODE_ABBR AND C.CODE_CATEGORY = 'CT' " &
                "WHERE PRD_COY_ID = '" & sCoyID & "' AND PRD_D_ADDR_CODE = '" & sAddrCode & "' AND PRD_PR_LINE_INDEX = '" & sPRLineIndex & "' "
            tDS = objDb.FillDs(tSQL)

            If tDS.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE1")) AndAlso tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE1") <> "" Then
                    sAddr = sAddr & " " & tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE1")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE2")) AndAlso tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE2") <> "" Then
                    sAddr = sAddr & ", " & tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE2")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE3")) AndAlso tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE3") <> "" Then
                    sAddr = sAddr & ", " & tDS.Tables(0).Rows(0).Item("AM_ADDR_LINE3")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_POSTCODE")) Then
                    sAddr = sAddr & ", " & tDS.Tables(0).Rows(0).Item("AM_POSTCODE")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_CITY")) AndAlso tDS.Tables(0).Rows(0).Item("AM_CITY") <> "" Then
                    sAddr = sAddr & " " & tDS.Tables(0).Rows(0).Item("AM_CITY")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_STATE")) Then
                    sAddr = sAddr & " " & tDS.Tables(0).Rows(0).Item("AM_STATE")
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("AM_COUNTRY")) Then
                    sAddr = sAddr & " " & tDS.Tables(0).Rows(0).Item("AM_COUNTRY")
                End If
            End If
            GetSpecificAddrPR = sAddr
        End Function

        Public Function CompanyParam() As Array
            Dim strAryComParam(0) As String
            Common.Insert2Ary(strAryComParam, "Select CP_PARAM_VALUE from COMPANY_PARAM where ")
        End Function
        'Zulham 16072018 - PAMB
        Public Function addDept(ByVal strDeptCode As String, ByVal strDeptName As String, ByVal strPaymentGrpIndex As String, Optional ByVal strIPPPaymentGrpIndex As String = "", Optional ByVal strPaymentGrpIndex_NR As String = "", Optional ByVal frm As String = "") As Integer
            Dim strSql_adddept As String
            Dim strCoyID, strUserId As String

            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")

            'Zulham 16072018 - PAMB
            'Zulham 11102018 - PAMB
            If frm <> "moddept" Then
                'checks for resident group
                If objDb.Exist("Select '*' From COMPANY_DEPT_MSTR, APPROVAL_GRP_MSTR Where CDM_DELETED = 'N' AND AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX AND AGM_RESIDENT = 'Y' AND CDM_COY_ID='" & strCoyID & "' And CDM_DEPT_CODE='" & Common.Parse(strDeptCode) & "' AND cdm_approval_grp_index = " & strPaymentGrpIndex) > 0 Then
                    addDept = WheelMsgNum.Duplicate
                    Exit Function
                ElseIf objDb.Exist("Select '*' From COMPANY_DEPT_MSTR, APPROVAL_GRP_MSTR Where CDM_DELETED = 'Y' AND AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX AND AGM_RESIDENT = 'Y' AND CDM_COY_ID='" & strCoyID & "' And CDM_DEPT_CODE='" & Common.Parse(strDeptCode) & "'  AND cdm_approval_grp_index = " & strPaymentGrpIndex) > 0 Then
                    'addDept = WheelMsgNum.Duplicate
                    If strPaymentGrpIndex <> "" Then
                        strSql_adddept = "UPDATE COMPANY_DEPT_MSTR set CDM_DELETED = 'N', " &
                                     "CDM_DEPT_NAME = '" & Common.Parse(strDeptName) & "', " &
                                     "CDM_APPROVAL_GRP_INDEX = '" & strPaymentGrpIndex & "' " &
                                     "Where CDM_COY_ID='" & strCoyID & "' And CDM_DEPT_CODE='" & Common.Parse(strDeptCode) & "'"
                    Else
                        strSql_adddept = "UPDATE COMPANY_DEPT_MSTR set CDM_DELETED = 'N', " &
                                     "CDM_DEPT_NAME = '" & Common.Parse(strDeptName) & "', " &
                                        "CDM_APPROVAL_GRP_INDEX = NULL " &
                                     "Where CDM_COY_ID='" & strCoyID & "' And CDM_DEPT_CODE='" & Common.Parse(strDeptCode) & "'"
                    End If

                    If objDb.Execute(strSql_adddept) Then
                        addDept = WheelMsgNum.Save
                    Else
                        addDept = WheelMsgNum.NotSave
                    End If

                    Exit Function
                End If

                'Zulham 16102018 - PAMB 
                If strPaymentGrpIndex <> "" And Not strPaymentGrpIndex = 0 Then
                    If strIPPPaymentGrpIndex = "" Then
                        strSql_adddept = "Insert Into COMPANY_DEPT_MSTR(CDM_COY_ID,CDM_DEPT_CODE, CDM_DEPT_NAME,CDM_ENT_BY,CDM_ENT_DT, CDM_APPROVAL_GRP_INDEX)values ('" &
                                        strCoyID & "','" & Common.Parse(strDeptCode) & "','" & Common.Parse(strDeptName) &
                                        "','" & strUserId & "'," & Common.ConvertDate(Now) & ", '" & strPaymentGrpIndex & "')"
                    Else
                        strSql_adddept = "Insert Into COMPANY_DEPT_MSTR(CDM_COY_ID,CDM_DEPT_CODE, CDM_DEPT_NAME,CDM_ENT_BY,CDM_ENT_DT, CDM_APPROVAL_GRP_INDEX,CDM_IPP_APPROVAL_GRP_INDEX)values ('" &
                                        strCoyID & "','" & Common.Parse(strDeptCode) & "','" & Common.Parse(strDeptName) &
                                        "','" & strUserId & "'," & Common.ConvertDate(Now) & ", '" & strPaymentGrpIndex & "','" & strIPPPaymentGrpIndex & "')"
                    End If
                ElseIf strIPPPaymentGrpIndex <> "" Then
                    strSql_adddept = "Insert Into COMPANY_DEPT_MSTR(CDM_COY_ID,CDM_DEPT_CODE, CDM_DEPT_NAME,CDM_ENT_BY,CDM_ENT_DT,CDM_IPP_APPROVAL_GRP_INDEX)values ('" &
                                        strCoyID & "','" & Common.Parse(strDeptCode) & "','" & Common.Parse(strDeptName) &
                                        "','" & strUserId & "'," & Common.ConvertDate(Now) & ",'" & strIPPPaymentGrpIndex & "')"
                Else
                    If Not strPaymentGrpIndex = 0 Then
                        strSql_adddept = "Insert Into COMPANY_DEPT_MSTR(CDM_COY_ID,CDM_DEPT_CODE, CDM_DEPT_NAME,CDM_ENT_BY,CDM_ENT_DT)values ('" &
                        strCoyID & "','" & Common.Parse(strDeptCode) & "','" & Common.Parse(strDeptName) &
                        "','" & strUserId & "'," & Common.ConvertDate(Now) & ")"
                    End If
                End If
                'Zulham 16102018 - PAMB 
                If Not strSql_adddept Is Nothing Then
                    If objDb.Execute(strSql_adddept) Then
                        addDept = WheelMsgNum.Save
                    Else
                        addDept = WheelMsgNum.NotSave
                    End If
                End If

            End If

            'Zulham 11102018 - PAMB 
            'validation for non-resident
            If objDb.Exist("Select '*' From COMPANY_DEPT_MSTR, APPROVAL_GRP_MSTR Where CDM_DELETED = 'N' AND AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX AND AGM_RESIDENT = 'N' AND CDM_COY_ID='" & strCoyID & "' And CDM_DEPT_CODE='" & Common.Parse(strDeptCode) & "' AND cdm_approval_grp_index = " & strPaymentGrpIndex_NR) > 0 Then
                'Zulham 14082018 - PAPMB
                addDept = WheelMsgNum.Duplicate '& " for Non-Resident Group."
                Exit Function
            ElseIf objDb.Exist("Select '*' From COMPANY_DEPT_MSTR, APPROVAL_GRP_MSTR Where CDM_DELETED = 'Y' AND AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX AND AGM_RESIDENT = 'N' AND CDM_COY_ID='" & strCoyID & "' And CDM_DEPT_CODE='" & Common.Parse(strDeptCode) & "' AND cdm_approval_grp_index = " & strPaymentGrpIndex_NR) > 0 Then
                'addDept = WheelMsgNum.Duplicate
                If strPaymentGrpIndex <> "" Then
                    strSql_adddept = "UPDATE COMPANY_DEPT_MSTR set CDM_DELETED = 'N', " &
                                     "CDM_DEPT_NAME = '" & Common.Parse(strDeptName) & "', " &
                                     "CDM_APPROVAL_GRP_INDEX = '" & strPaymentGrpIndex_NR & "' " &
                                     "Where CDM_COY_ID='" & strCoyID & "' And CDM_DEPT_CODE='" & Common.Parse(strDeptCode) & "'"
                Else
                    strSql_adddept = "UPDATE COMPANY_DEPT_MSTR set CDM_DELETED = 'N', " &
                                     "CDM_DEPT_NAME = '" & Common.Parse(strDeptName) & "', " &
                                     "CDM_APPROVAL_GRP_INDEX = NULL " &
                                     "Where CDM_COY_ID='" & strCoyID & "' And CDM_DEPT_CODE='" & Common.Parse(strDeptCode) & "'"
                End If

                If objDb.Execute(strSql_adddept) Then
                    addDept = WheelMsgNum.Save
                Else
                    addDept = WheelMsgNum.NotSave
                End If

                Exit Function
            End If

            If strPaymentGrpIndex_NR <> 0 And strPaymentGrpIndex_NR.Trim <> "" Then
                strSql_adddept = "Insert Into COMPANY_DEPT_MSTR(CDM_COY_ID,CDM_DEPT_CODE, CDM_DEPT_NAME,CDM_ENT_BY,CDM_ENT_DT, CDM_APPROVAL_GRP_INDEX)values ('" &
                                strCoyID & "','" & Common.Parse(strDeptCode) & "','" & Common.Parse(strDeptName) &
                                "','" & strUserId & "'," & Common.ConvertDate(Now) & ", '" & strPaymentGrpIndex_NR & "')"

                If objDb.Execute(strSql_adddept) Then
                    addDept = WheelMsgNum.Save
                Else
                    addDept = WheelMsgNum.NotSave
                End If
            End If
            'End

        End Function
        'Zulham 16072018 - PAMB
        Public Function moddept(ByVal strDeptCode As String, ByVal strDeptName As String, ByVal strOld As String, ByVal strPaymentGrpIndex As String, Optional ByVal strIPPPaymentGrpIndex As String = "", Optional ByVal strPaymentGrpIndex_NR As String = "") As Integer
            Dim strSql_moddept As String
            'Zulham 18072018 - PAMB
            Dim status = 0

            If UCase(strOld) <> UCase(strDeptName) Then
                If objDb.Exist("Select '*' From COMPANY_DEPT_MSTR Where CDM_COY_ID ='" &
                  HttpContext.Current.Current.Session("CompanyID") & "' and CDM_DEPT_NAME ='" &
                  Common.Parse(strDeptName) & "'") > 0 Then
                    moddept = WheelMsgNum.Duplicate
                    Exit Function
                End If
            End If
            'Zulham 16072018 - PAMB
            'Zulham 11102018 - PAMB SST
            'Check if group already registered
            'Resident
            If objDb.Exist("Select '*' From APPROVAL_GRP_MSTR JOIN COMPANY_DEPT_MSTR ON AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX WHERE AGM_RESIDENT ='Y' AND CDM_DEPT_CODE = '" & Common.Parse(strDeptCode) & "' AND cdm_approval_grp_index = " & strPaymentGrpIndex & "") > 0 Then
                'get record index
                Dim idx = objDb.GetVal("Select cdm_dept_index From APPROVAL_GRP_MSTR JOIN COMPANY_DEPT_MSTR ON AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX WHERE AGM_RESIDENT ='Y' AND CDM_DEPT_CODE = '" & Common.Parse(strDeptCode) & "' AND cdm_approval_grp_index = " & strPaymentGrpIndex & "")
                'edit existing data
                strSql_moddept = "UPDATE COMPANY_DEPT_MSTR set CDM_DEPT_NAME ='" & Common.Parse(strDeptName) &
                                "',CDM_MOD_BY='" & HttpContext.Current.Session("UserId") & "',CDM_MOD_DT=" &
                                Common.ConvertDate(Now) & ""
                If strPaymentGrpIndex <> "" Then
                    strSql_moddept &= ", CDM_APPROVAL_GRP_INDEX='" & strPaymentGrpIndex & "' "
                End If
                If strIPPPaymentGrpIndex <> "" Then
                    strSql_moddept &= ", CDM_IPP_APPROVAL_GRP_INDEX='" & strIPPPaymentGrpIndex & "' "
                End If
                strSql_moddept &= " where CDM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CDM_DEPT_CODE = '" & Common.Parse(strDeptCode) & "' and cdm_dept_index =" & idx


                If objDb.Execute(strSql_moddept) Then
                    moddept = WheelMsgNum.Save
                Else
                    moddept = WheelMsgNum.NotSave
                End If
            Else
                'add new group
                status = addDept(strDeptCode, strDeptName, strPaymentGrpIndex, strIPPPaymentGrpIndex, 0) 'Zulham 18072018 - PAMB
                'Zulham 01112018
                moddept = status
            End If

            'Zulham 11102018 - PAMB 
            'Non-resident
            If objDb.Exist("Select '*' From APPROVAL_GRP_MSTR JOIN COMPANY_DEPT_MSTR ON AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX WHERE AGM_RESIDENT ='N' AND CDM_DEPT_CODE = '" & Common.Parse(strDeptCode) & "'  AND cdm_approval_grp_index = " & strPaymentGrpIndex_NR & "") > 0 Then
                'get record index
                Dim idx = objDb.GetVal("Select cdm_dept_index From APPROVAL_GRP_MSTR JOIN COMPANY_DEPT_MSTR ON AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX WHERE AGM_RESIDENT ='N' AND CDM_DEPT_CODE = '" & Common.Parse(strDeptCode) & "' AND cdm_approval_grp_index = " & strPaymentGrpIndex_NR & "")
                'edit existing data
                strSql_moddept = "UPDATE COMPANY_DEPT_MSTR set CDM_DEPT_NAME ='" & Common.Parse(strDeptName) &
                                "',CDM_MOD_BY='" & HttpContext.Current.Session("UserId") & "',CDM_MOD_DT=" &
                                Common.ConvertDate(Now) & ""
                If strPaymentGrpIndex_NR <> "" Then
                    If strPaymentGrpIndex_NR <> 0 Then
                        strSql_moddept &= ", CDM_APPROVAL_GRP_INDEX='" & strPaymentGrpIndex_NR & "' "
                    End If
                End If
                If strIPPPaymentGrpIndex <> "" Then
                    strSql_moddept &= ", CDM_IPP_APPROVAL_GRP_INDEX='" & strIPPPaymentGrpIndex & "' "
                End If
                strSql_moddept &= " where CDM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CDM_DEPT_CODE = '" & Common.Parse(strDeptCode) & "' and cdm_dept_index =" & idx


                If objDb.Execute(strSql_moddept) Then
                    moddept = WheelMsgNum.Save
                Else
                    moddept = WheelMsgNum.NotSave
                End If
            Else
                'add new group
                status = addDept(strDeptCode, strDeptName, strPaymentGrpIndex, strIPPPaymentGrpIndex, strPaymentGrpIndex_NR, "moddept") 'Zulham 18072018 - PAMB
            End If

            'Zulham 18072018 - PAMB
            If status <> 0 Then
                Return status
            Else
                Return moddept
            End If

        End Function

        'Public Function deldept(ByVal strDeptCode As String) As Integer
        '    Dim strSql_deldept As String
        '    Dim strSql_remaindept As String
        '    '//need to check whether got BCM before deletion
        '    strSql_deldept = "DELETE FROM COMPANY_DEPT_MSTR where CDM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' and CDM_DEPT_CODE = '" & strDeptCode & "'"
        '    If objDb.Execute(strSql_deldept) Then
        '        deldept = WheelMsgNum.Delete
        '    Else
        '        deldept = WheelMsgNum.NotDelete
        '    End If
        'End Function
        'meilai delete dept based on some condition
        Public Function delCdept(ByVal dtDept As DataTable) As Integer
            Dim strAry(0) As String
            Dim strSQL, strUserID As String
            Dim drDept As DataRow
            strUserID = HttpContext.Current.Session("UserId")

            For Each drDept In dtDept.Rows
                strSQL = "select '*' from ACCOUNT_MSTR WHERE AM_DEPT_INDEX=" & Common.Parse(drDept("Dept_Index")) & " And AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' and AM_DELETED = 'N' "
                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                strSQL = "select '*' from USER_MSTR WHERE UM_DEPT_ID='" & Common.Parse(drDept("Dept_code")) & "' And UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' and UM_DELETED <> 'Y' AND UM_STATUS='A'"
                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If
                'AND CDM_DEPT_CODE = '" & strDeptCode & "' AND UM_USER_ID = '" & strUserID & "' AND UM_STATUS='A'"

                strSQL = "update company_dept_mstr set cdm_deleted='Y' where cdm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' and cdm_dept_code='" & Common.Parse(drDept("Dept_code")) & "'"
                Common.Insert2Ary(strAry, strSQL)
            Next
            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function
        Public Function getdept() As DataSet
            Dim ds As DataSet
            Dim sql As String
            'zULHAM 18072018 - PAMB
            sql = "SELECT DISTINCT CDM_DEPT_CODE,CDM_DEPT_NAME FROM COMPANY_DEPT_MSTR WHERE CDM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND CDM_DELETED='N' ORDER BY CDM_DEPT_NAME ASC"
            ds = objDb.FillDs(sql)
            Return ds
        End Function
        Public Function searchdept(ByVal strDeptCode As String, ByVal strDeptName As String, Optional ByVal blnSortDeptName As Boolean = False) As DataSet
            Dim strsql_sdept As String
            Dim dssdept As DataSet
            Dim strTemp As String

            'Zulham 18072018 - PAMB
            ' strsql_sdept = "select CDM_DEPT_INDEX,CDM_DEPT_CODE, CDM_DEPT_NAME, AGM_GRP_NAME, CAST(CASE WHEN CDM_APPROVAL_GRP_INDEX IS NULL THEN '' ELSE CDM_APPROVAL_GRP_INDEX END AS SIGNED) AS CDM_APPROVAL_GRP_INDEX " &
            '" from COMPANY_DEPT_MSTR LEFT JOIN APPROVAL_GRP_MSTR ON CDM_APPROVAL_GRP_INDEX = AGM_GRP_INDEX WHERE CDM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND CDM_DELETED='N'"
            strsql_sdept = "SELECT * FROM (Select CDM1.CDM_DEPT_INDEX,CDM1.CDM_DEPT_CODE, CDM1.CDM_DEPT_NAME, CONCAT(AGM1.AGM_GRP_NAME,'  /  ',AGM2.AGM_GRP_NAME) 'AGM_GRP_NAME', " &
                        "CAST(CASE WHEN CDM1.CDM_APPROVAL_GRP_INDEX Is NULL THEN '' ELSE CDM1.CDM_APPROVAL_GRP_INDEX END AS SIGNED) AS CDM_APPROVAL_GRP_INDEX " &
                        "From COMPANY_DEPT_MSTR CDM1 " &
                        "Left Join APPROVAL_GRP_MSTR AGM1 ON CDM1.CDM_APPROVAL_GRP_INDEX = AGM1.AGM_GRP_INDEX  " &
                        "Left Join COMPANY_DEPT_MSTR CDM2 ON CDM1.CDM_DEPT_CODE = CDM2.CDM_DEPT_CODE " &
                        "Left Join APPROVAL_GRP_MSTR AGM2 ON CDM2.CDM_APPROVAL_GRP_INDEX = AGM2.AGM_GRP_INDEX " &
                        "WHERE CDM1.CDM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CDM1.CDM_DELETED='N' " &
                        "And AGM1.AGM_RESIDENT IN ('Y') " &
                        "And AGM2.AGM_RESIDENT IN ('N') " &
                            "UNION ALL " &
                        "Select CDM1.CDM_DEPT_INDEX,CDM1.CDM_DEPT_CODE, CDM1.CDM_DEPT_NAME, AGM1.AGM_GRP_NAME, " &
                        "CAST(CASE WHEN CDM1.CDM_APPROVAL_GRP_INDEX Is NULL THEN '' ELSE CDM1.CDM_APPROVAL_GRP_INDEX END AS SIGNED) AS CDM_APPROVAL_GRP_INDEX " &
                        "From COMPANY_DEPT_MSTR CDM1 " &
                        "Left Join APPROVAL_GRP_MSTR AGM1 ON CDM_APPROVAL_GRP_INDEX = AGM1.AGM_GRP_INDEX " &
                        "WHERE CDM1.CDM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CDM1.CDM_DELETED='N' " &
                        "GROUP BY CDM_DEPT_CODE " &
                        "HAVING COUNT(CDM_DEPT_CODE) = 1 " &
                        ") ZZZ "

            If strDeptCode <> "" Then
                strTemp = Common.BuildWildCard(strDeptCode)
                strsql_sdept = strsql_sdept & " WHERE CDM_DEPT_CODE" & Common.ParseSQL(strTemp)
            End If

            If strDeptName <> "" And strDeptCode <> "" Then
                strTemp = Common.BuildWildCard(strDeptName)
                strsql_sdept = strsql_sdept & " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
            ElseIf strDeptName <> "" Then
                strTemp = Common.BuildWildCard(strDeptName)
                strsql_sdept = strsql_sdept & " WHERE CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
            End If

            ''Zulham 12102018 - APMB
            strsql_sdept += " GROUP BY CDM_DEPT_NAME"

            '//this query will sort by dept code by default
            '//some screen may only show dept name, so need to sort by dept name
            If blnSortDeptName Then
                strsql_sdept += " order by CDM_DEPT_NAME"
            End If
            dssdept = objDb.FillDs(strsql_sdept)
            searchdept = dssdept
        End Function
        'mimi : 21/03/2017 - enhancement smart pay ref.
        Public Function updateParam(ByVal htPageAccess As Hashtable, ByVal intRFQOption As Integer, ByVal intFreeForm As Integer, ByVal intConsolidation As Integer, ByVal intLevelsRec As Integer, ByVal intConOption As String, ByVal PO As String, Optional ByVal intNConOption As String = "", Optional ByVal intGRNOption As Integer = 1, Optional ByVal strAccCode As String = "N", Optional ByVal aryIQC As ArrayList = Nothing _
                                    , Optional ByVal strActivateStk As String = "", Optional ByVal strUrgentStk As String = "N", Optional ByVal strRejStk As String = "N", Optional ByVal strSafetyStk As String = "N", Optional ByVal strReorderStk As String = "N", Optional ByVal strMaxStk As String = "N", Optional ByVal strLocStk As String = "N", Optional ByVal intSmartPay As Decimal = 0)
            Dim myEnumerator As IDictionaryEnumerator = htPageAccess.GetEnumerator()
            Dim objDb As New EAD.DBCom
            Dim strUpdate As String
            Dim strValue As String
            Dim strPrefix As String
            Dim strAryQuery(0) As String
            Dim strsql As String

            While myEnumerator.MoveNext()
                If myEnumerator.Key = "" Then
                    Exit While
                End If
                strValue = myEnumerator.Value
                If Right(myEnumerator.Key, 6).Trim.ToUpper = "PREFIX" Then
                    strPrefix = Left(myEnumerator.Key, Len(myEnumerator.Key) - 6).Trim
                    If objDb.Exist("SELECT '*' FROM COMPANY_PARAM WHERE CP_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CP_PARAM_TYPE = '" & strPrefix & "' AND CP_PARAM_NAME = 'Prefix'") > 0 Then
                        strUpdate = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE = '" & Common.Parse(strValue) & "' "
                        strUpdate &= "WHERE CP_PARAM_TYPE = '" & strPrefix & "' "
                        strUpdate &= "AND CP_PARAM_NAME = 'Prefix' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                    Else
                        strUpdate = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_VALUE, CP_PARAM_TYPE, CP_PARAM_NAME) VALUES ("
                        strUpdate &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                        strUpdate &= "'" & Common.Parse(strValue) & "', "
                        strUpdate &= "'" & strPrefix & "', "
                        strUpdate &= "'Prefix')"
                    End If

                Else
                    strPrefix = Left(myEnumerator.Key, Len(myEnumerator.Key) - 10).Trim
                    If objDb.Exist("SELECT '*' FROM COMPANY_PARAM WHERE CP_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CP_PARAM_TYPE = '" & strPrefix & "' AND CP_PARAM_NAME = 'Last Used No'") > 0 Then
                        strUpdate = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE = '" & Common.Parse(strValue) & "' "
                        strUpdate &= "WHERE CP_PARAM_TYPE = '" & strPrefix & "' "
                        strUpdate &= "AND CP_PARAM_NAME = 'Last Used No' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                    Else
                        strUpdate = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_VALUE, CP_PARAM_TYPE, CP_PARAM_NAME) VALUES ("
                        strUpdate &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                        strUpdate &= "'" & Common.Parse(strValue) & "', "
                        strUpdate &= "'" & strPrefix & "', "
                        strUpdate &= "'Last Used No')"
                    End If
                End If
                Common.Insert2Ary(strAryQuery, strUpdate)
            End While

            'Dim strSql As String
            strsql = "update COMPANY_MSTR set CM_CONTR_PR_PO_OWNER_ID='" & Common.Parse(PO) & "', CM_CONTR_PR_SETTING='" & Common.Parse(intConOption) & "', CM_RFQ_OPTION='" & Common.Parse(intRFQOption) & "', CM_DISPLAY_ACCT='" & Common.Parse(strAccCode) & "' "
            ' strSql &= "CM_PAYMENT_TERM='" & Common.Parse(strPaymentTerm) & "', CM_PAYMENT_METHOD='" & Common.Parse(strPaymentMethod) & "', CM_PWD_DURATION='" & Common.Parse(strPaymentDuration) & "' "
            strsql &= "WHERE CM_COY_ID= '" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strAryQuery, strsql)

            ''Michelle (10/1/2013) - Issue 1832
            strsql = "update COMPANY_MSTR set CM_ACTIVATE_STOCK=" & IIf(strActivateStk = "", "NULL", "'" & strActivateStk & "'") & " "
            strsql &= ", CM_URGENT_STOCK_EMAIL='" & Common.Parse(strUrgentStk) & "' "
            strsql &= ", CM_REJECT_STOCK_EMAIL='" & Common.Parse(strRejStk) & "' "
            strsql &= ", CM_SAFETY_STOCK_EMAIL='" & Common.Parse(strSafetyStk) & "' "
            strsql &= ", CM_REORDER_STOCK_EMAIL='" & Common.Parse(strReorderStk) & "' "
            strsql &= ", CM_MAXIMUM_STOCK_EMAIL='" & Common.Parse(strMaxStk) & "' "
            strsql &= ", CM_LOCATION_STOCK='" & Common.Parse(strLocStk) & "' "
            strsql &= ", CM_SMART_PAY='" & Common.Parse(intSmartPay) & "' " 'mimi : 21/03/2017 - enhancement smart pay ref.
            'strsql &= ", CM_SMART_PAY='" & IIf(intSmartPay = "", "NULL", "'" & intSmartPay & "'") & " " 'mimi : 21/03/2017 - enhancement smart pay ref.
            strsql &= "WHERE CM_COY_ID= '" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strAryQuery, strsql)

            'Chee Hong (03/06/2013) - Issue 1832
            'strsql = "update COMPANY_MSTR set CM_NCONTR_PR_SETTING='" & Common.Parse(intNConOption) & "' "
            strsql = "update COMPANY_MSTR set CM_NCONTR_PR_SETTING='" & Common.Parse(intNConOption) & "' "
            strsql &= ", CM_GRN_CONTROL='" & IIf(intGRNOption = 1, "N", "Y") & "' "
            strsql &= "WHERE CM_COY_ID= '" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strAryQuery, strsql)

            '//Carol add

            If objDb.Exist("SELECT '*' FROM COMPANY_SETTING WHERE CS_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CS_FLAG_NAME = 'Allow Free Form Billing Address'") > 0 Then
                strsql = "update COMPANY_SETTING set CS_FLAG_VALUE ='" & Common.Parse(intFreeForm) & "' "
                strsql &= "where CS_FLAG_NAME = 'Allow Free Form Billing Address' "
                strsql &= "AND CS_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Else
                strsql = "INSERT INTO COMPANY_SETTING(CS_COY_ID, CS_FLAG_NAME, CS_FLAG_VALUE, CS_FLAG_TYPE) VALUES ("
                strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                strsql &= "'Allow Free Form Billing Address', "
                strsql &= "'" & Common.Parse(intFreeForm) & "', "
                strsql &= "'CoyParam')"
            End If
            'strsql = "update COMPANY_SETTING set CS_FLAG_VALUE ='" & Common.Parse(intFreeForm) & "' where CS_FLAG_NAME = 'Allow Free Form Billing Address' AND CS_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strAryQuery, strsql)

            '//Carol add
            If objDb.Exist("SELECT '*' FROM COMPANY_SETTING WHERE CS_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CS_FLAG_NAME = 'Consolidation Required'") > 0 Then
                strsql = "update COMPANY_SETTING set CS_FLAG_VALUE ='" & Common.Parse(intConsolidation) & "' "
                strsql &= "where CS_FLAG_NAME = 'Consolidation Required' "
                strsql &= "AND CS_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Else
                strsql = "INSERT INTO COMPANY_SETTING(CS_COY_ID, CS_FLAG_NAME, CS_FLAG_VALUE, CS_FLAG_TYPE) VALUES ("
                strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                strsql &= "'Consolidation Required', "
                strsql &= "'" & Common.Parse(intConsolidation) & "', "
                strsql &= "'CoyParam')"
            End If
            'strsql = "update COMPANY_SETTING set CS_FLAG_VALUE ='" & Common.Parse(intConsolidation) & "' where CS_FLAG_NAME = 'Consolidation Required' AND CS_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strAryQuery, strsql)


            If objDb.Exist("SELECT '*' FROM COMPANY_SETTING WHERE CS_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CS_FLAG_NAME = '2 Level Receiving'") > 0 Then
                strsql = "update COMPANY_SETTING set CS_FLAG_VALUE ='" & Common.Parse(intLevelsRec) & "' "
                strsql &= "where CS_FLAG_NAME = '2 Level Receiving' "
                strsql &= "AND CS_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Else
                strsql = "INSERT INTO COMPANY_SETTING(CS_COY_ID, CS_FLAG_NAME, CS_FLAG_VALUE, CS_FLAG_TYPE) VALUES ("
                strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                strsql &= "'2 Level Receiving', "
                strsql &= "'" & Common.Parse(intLevelsRec) & "', "
                strsql &= "'CoyParam')"
            End If
            ' strsql = "update COMPANY_SETTING set CS_FLAG_VALUE ='" & Common.Parse(intLevelsRec) & "' where CS_FLAG_NAME = '2 Level Receiving' AND CS_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strAryQuery, strsql)

            'Chee Hong
            'IQC Test Type Setting
            Dim i As Integer
            If Not aryIQC Is Nothing Then
                If aryIQC.Count > 0 Then


                    strsql = ""

                    'Delete row with selected index
                    For i = 0 To aryIQC.Count - 1
                        If aryIQC(i)(0) <> "" And aryIQC(i)(1) <> "" And aryIQC(i)(2) <> "" And aryIQC(i)(4) <> "" Then
                            If strsql = "" Then
                                strsql = "CPA_INDEX <> '" & aryIQC(i)(4) & "' "
                            Else
                                strsql &= "AND CPA_INDEX <> '" & aryIQC(i)(4) & "' "
                            End If

                        End If
                    Next

                    If strsql <> "" Then
                        strsql = "DELETE FROM COMPANY_PARAM_ADDITIONAL WHERE CPA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (" & strsql & ")"
                        Common.Insert2Ary(strAryQuery, strsql)
                    End If

                    For i = 0 To aryIQC.Count - 1
                        If aryIQC(i)(0) <> "" And aryIQC(i)(1) <> "" And aryIQC(i)(2) <> "" And aryIQC(i)(4) <> "" Then
                            'Update row with selected index
                            strsql = "UPDATE COMPANY_PARAM_ADDITIONAL SET "
                            strsql &= "CPA_PARAM_LABEL = '" & Common.Parse(aryIQC(i)(0)) & "', "
                            strsql &= "CPA_PARAM_PREFIX = '" & Common.Parse(aryIQC(i)(1)) & "', "
                            strsql &= "CPA_PARAM_VALUE = '" & aryIQC(i)(2) & "', "
                            strsql &= "CPA_PARAM_ATTACHMENT = '" & IIf(aryIQC(i)(3) = "on", "Y", "N") & "' "
                            strsql &= "WHERE CPA_INDEX = '" & aryIQC(i)(4) & "' "

                            Common.Insert2Ary(strAryQuery, strsql)
                        ElseIf aryIQC(i)(0) <> "" And aryIQC(i)(1) <> "" And aryIQC(i)(2) <> "" And (aryIQC(i)(4) = "" Or aryIQC(i)(4) = Nothing) Then
                            'Add new row without index
                            strsql = "INSERT INTO COMPANY_PARAM_ADDITIONAL(CPA_COY_ID,CPA_PARAM_LABEL,CPA_PARAM_PREFIX,CPA_PARAM_VALUE,CPA_PARAM_TYPE,CPA_PARAM_ATTACHMENT) VALUES ("
                            strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                            strsql &= "'" & Common.Parse(aryIQC(i)(0)) & "', "
                            strsql &= "'" & Common.Parse(aryIQC(i)(1)) & "', "
                            strsql &= "'" & aryIQC(i)(2) & "', "
                            strsql &= "'IQC', "
                            strsql &= "'" & IIf(aryIQC(i)(3) = "on", "Y", "N") & "')"

                            Common.Insert2Ary(strAryQuery, strsql)
                        End If

                    Next

                    'For i = 0 To aryIQC.Count - 1
                    '    If aryIQC(i)(0) <> "" And aryIQC(i)(1) <> "" And aryIQC(i)(2) <> "" Then
                    '        strsql = "INSERT INTO COMPANY_PARAM_ADDITIONAL(CPA_COY_ID,CPA_PARAM_LABEL,CPA_PARAM_PREFIX,CPA_PARAM_VALUE,CPA_PARAM_TYPE,CPA_PARAM_ATTACHMENT) VALUES ("
                    '        strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                    '        strsql &= "'" & Common.Parse(aryIQC(i)(0)) & "', "
                    '        strsql &= "'" & Common.Parse(aryIQC(i)(1)) & "', "
                    '        strsql &= "'" & aryIQC(i)(2) & "', "
                    '        strsql &= "'IQC', "
                    '        strsql &= "'" & IIf(aryIQC(i)(3) = "on", "Y", "N") & "')"

                    '        Common.Insert2Ary(strAryQuery, strsql)
                    '    End If
                    'Next
                End If
            End If


            objDb.BatchExecute(strAryQuery)
        End Function

        'Public Function updateParamMaster(ByVal strRFQOption As String, ByVal strPaymentTerm As String, ByVal strPaymentMethod As String, _
        'ByVal strPaymentDuration As String, ByVal intFreeForm As Integer, ByVal intConsolidation As Integer)
        '    Dim objDb As New EAD.DBCom()
        '    Dim strSql As String
        '    Dim strAryQuery(0) As String
        '    strSql = "update COMPANY_MSTR set CM_RFQ_OPTION='" & Common.Parse(strRFQOption) & "', "
        '    strSql &= "CM_PAYMENT_TERM='" & Common.Parse(strPaymentTerm) & "', CM_PAYMENT_METHOD='" & Common.Parse(strPaymentMethod) & "', CM_PWD_DURATION='" & Common.Parse(strPaymentDuration) & "', "
        '    Common.Insert2Ary(strAryQuery, strSql)
        '    strSql = "update COMPANY_SETTING set CS_FLAG_VALUE ='" & intFreeForm & "' where CS_FLAG_NAME = 'Allow Free Form Billing Address' "
        '    Common.Insert2Ary(strAryQuery, strSql)
        '    strSql = "update COMPANY_SETTING set CS_FLAG_VALUE ='" & intConsolidation & "' where CS_FLAG_NAME = 'Consolidation Needed' "
        '    Common.Insert2Ary(strAryQuery, strSql)
        '    objDb.BatchExecute(strAryQuery)
        '    'If objDb.Execute(strSql_updateParamMaster) Then
        '    '    updateParamMaster = RecordSave
        '    'Else
        '    '    updateParamMaster = RecordNotSave
        '    'End If
        'End Function




        'Public Function getParamMaster() As DataSet
        '    Dim objDb As New EAD.DBCom()
        '    Dim strget As String
        '    Dim dsparam As New DataSet()
        '    strget = "Select CM_RFQ_OPTION,CM_PAYMENT_TERM,CM_PAYMENT_METHOD,CM_PWD_DURATION from COMPANY_MSTR where CM_COY_ID = 'demo'; "
        '    strget &= "Select CS_FLAG_NAME,CS_FLAG_VALUE from COMPANY_SETTING where CS_FLAG_TYPE= 'CoyParam'"
        '    dsparam = objDb.FillDs(strget)

        '    getParamMaster = dsparam
        'End Function


        Public Function getAllowFreeForm() As Boolean
            Dim strSql As String
            Dim dsFree As New DataSet
            strSql = "SELECT CS_FLAG_VALUE FROM COMPANY_SETTING WHERE CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSql &= "AND CS_FLAG_TYPE = 'CoyParam' "
            strSql &= "AND CS_FLAG_NAME = 'Allow Free Form Billing Address' "

            dsFree = objDb.FillDs(strSql)
            If dsFree.Tables(0).Rows.Count > 0 Then
                If dsFree.Tables(0).Rows(0)(0) = "1" Then
                    getAllowFreeForm = True
                Else
                    getAllowFreeForm = False
                End If
            End If
        End Function

        Public Function getParam() As DataSet
            Dim objDb As New EAD.DBCom
            Dim strGet As String
            Dim dsParam As New DataSet
            strGet = "SELECT CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE FROM COMPANY_PARAM WHERE CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "
            'Dim strget As String
            'Dim dsparam As New DataSet()

            'mimi : 21/03/2017 - enhancement smart pay ref.
            'Chee Hong (10/1/2013) - Issue 1832
            strGet = strGet & "Select CM_RFQ_OPTION, CM_CONTR_PR_SETTING, CM_CONTR_PR_PO_OWNER_ID, IFNULL(CM_NCONTR_PR_SETTING,'') AS CM_NCONTR_PR_SETTING, CM_DISPLAY_ACCT, "
            strGet &= "CM_ACTIVATE_STOCK, CM_URGENT_STOCK_EMAIL, CM_REJECT_STOCK_EMAIL, CM_SAFETY_STOCK_EMAIL, CM_REORDER_STOCK_EMAIL, CM_MAXIMUM_STOCK_EMAIL, CM_LOCATION_STOCK, "
            strGet &= "(CASE WHEN CM_GRN_CONTROL = 'Y' THEN 0 ELSE 1 END) AS CM_GRN_CONTROL, CM_SMART_PAY from COMPANY_MSTR where CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "
            strGet &= "Select CS_FLAG_NAME,CS_FLAG_VALUE from COMPANY_SETTING where CS_FLAG_TYPE= 'CoyParam' AND CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            'strGet = strGet & "Select CM_RFQ_OPTION, CM_CONTR_PR_SETTING, CM_CONTR_PR_PO_OWNER_ID, IFNULL(CM_NCONTR_PR_SETTING,'') AS CM_NCONTR_PR_SETTING, CM_DISPLAY_ACCT, (CASE WHEN CM_GRN_CONTROL = 'Y' THEN 0 ELSE 1 END) AS CM_GRN_CONTROL from COMPANY_MSTR where CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "
            'strGet &= "Select CS_FLAG_NAME,CS_FLAG_VALUE from COMPANY_SETTING where CS_FLAG_TYPE= 'CoyParam' AND CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            dsParam = objDb.FillDs(strGet)
            getParam = dsParam
        End Function

        Public Function addAddress(ByVal strCode As String, ByVal strAdd1 As String, ByVal strAdd2 As String,
            ByVal strAdd3 As String, ByVal strPostCode As String, ByVal strCity As String,
            ByVal strState As String, ByVal strCountry As String, ByVal strType As String) As Integer
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim strCoyID As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strsql = "Select * From ADDRESS_MSTR Where AM_COY_ID = '" & Common.Parse(strCoyID) & "' AND AM_ADDR_CODE = '" & Common.Parse(strCode) & "' AND AM_ADDR_TYPE = '" & Common.Parse(strType) & "'"

            If objDb.Exist(strsql) > 0 Then
                addAddress = WheelMsgNum.Duplicate
                Exit Function
            End If

            'Michelle 23/9/2010 - To change syntax to cater for MYSQL
            'strsql = "INSERT INTO ADDRESS_MSTR VALUES("
            strsql = "INSERT INTO ADDRESS_MSTR (AM_COY_ID, AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, AM_STATE, AM_COUNTRY, AM_ADDR_TYPE) VALUES("
            strsql &= "'" & Common.Parse(strCoyID) & "', "
            strsql &= "'" & Common.Parse(strCode) & "', "
            strsql &= "'" & Common.Parse(strAdd1) & "', "
            strsql &= "'" & Common.Parse(strAdd2) & "', "
            strsql &= "'" & Common.Parse(strAdd3) & "', "
            strsql &= "'" & Common.Parse(strPostCode) & "', "
            strsql &= "'" & Common.Parse(strCity) & "', "
            strsql &= "'" & Common.Parse(strState) & "', "
            strsql &= "'" & Common.Parse(strCountry) & "', "
            strsql &= "'" & Common.Parse(strType) & "')"
            Common.Insert2Ary(strAryQuery, strsql)

            ' ai chu modified on 12/11/2005
            ' need to check if exists SK/2nd that is/are authorized to all Delivery Address(es)
            ' for that company, then need to add a record to USERS_LOCATION table 
            ' USERS_LOCATION - UL_COY_ID,UL_ADDR_CODE,UL_USER_ID,UL_LEVEL
            Dim ds As New DataSet
            Dim i As Integer
            strsql = "SELECT UL_USER_ID, UL_LEVEL FROM USERS_LOCATION "
            strsql &= "WHERE UL_COY_ID = '" & Common.Parse(strCoyID) & "' "
            strsql &= "GROUP BY UL_COY_ID, UL_USER_ID, UL_LEVEL HAVING COUNT(UL_ADDR_CODE) = "
            strsql &= "(SELECT COUNT(AM_ADDR_CODE) FROM ADDRESS_MSTR "
            strsql &= "WHERE AM_ADDR_TYPE='D' AND AM_COY_ID='" & Common.Parse(strCoyID) & "')"
            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    strsql = "INSERT INTO USERS_LOCATION (UL_COY_ID, UL_ADDR_CODE, UL_USER_ID, UL_LEVEL) "
                    strsql &= "VALUES ('" & Common.Parse(strCoyID) & "', "
                    strsql &= "'" & Common.Parse(strCode) & "', "
                    strsql &= "'" & Common.Parse(ds.Tables(0).Rows(i)("UL_USER_ID")) & "', "
                    strsql &= "'" & Common.Parse(ds.Tables(0).Rows(i)("UL_LEVEL")) & "')"
                    Common.Insert2Ary(strAryQuery, strsql)
                Next
            End If

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    addAddress = WheelMsgNum.Save
                Else
                    addAddress = WheelMsgNum.NotSave
                End If
            End If
        End Function


        Public Function addVendorReq(ByVal strReqCode As String, ByVal strReqDesc As String, ByVal strReqCategory As String, ByVal strStatus As String) As Integer

            Dim strCountry As String
            Dim strCoyID As String
            Dim strUserID As String

            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")
            If objDb.Exist("Select '*' From VENDOR_REQUEST Where VR_COY_ID='" & strCoyID &
            "' And VR_REQ_CODE='" & Common.Parse(strReqCode) & "' AND VR_REQ_CATEGORY='" & strReqCategory & "'") > 0 Then
                addVendorReq = WheelMsgNum.Duplicate
                Exit Function
            End If

            strCountry = "INSERT INTO VENDOR_REQUEST (VR_COY_ID, VR_ENT_BY, VR_REQ_CODE, VR_REQ_DESC,VR_REQ_CATEGORY, VR_STATUS) VALUES("
            strCountry &= "'" & Common.Parse(strCoyID) & "', "
            strCountry &= "'" & Common.Parse(strUserID) & "', "
            strCountry &= "'" & Common.Parse(strReqCode) & "', "
            strCountry &= "'" & Common.Parse(strReqDesc) & "', "
            strCountry &= "'" & Common.Parse(strReqCategory) & "', "
            strCountry &= "'" & Common.Parse(strStatus) & "') "
            objDb.Execute(strCountry)

            If objDb.Execute(strCountry) Then
                addVendorReq = WheelMsgNum.Save
            Else
                addVendorReq = WheelMsgNum.NotSave
            End If
        End Function

        Public Function modAddress(ByVal strCode As String, ByVal strAdd1 As String, ByVal strAdd2 As String,
            ByVal strAdd3 As String, ByVal strPostCode As String, ByVal strCity As String,
            ByVal strState As String, ByVal strCountry As String, ByVal strType As String) As Integer
            Dim strAdd As String
            Dim strCoyID As String
            strCoyID = HttpContext.Current.Session("CompanyId")

            strAdd = "UPDATE ADDRESS_MSTR SET "
            strAdd &= "AM_ADDR_LINE1 = '" & Common.Parse(strAdd1) & "', "
            strAdd &= "AM_ADDR_LINE2 = '" & Common.Parse(strAdd2) & "', "
            strAdd &= "AM_ADDR_LINE3 = '" & Common.Parse(strAdd3) & "', "
            strAdd &= "AM_POSTCODE = '" & Common.Parse(strPostCode) & "', "
            strAdd &= "AM_CITY = '" & Common.Parse(strCity) & "', "
            strAdd &= "AM_STATE = '" & Common.Parse(strState) & "', "
            strAdd &= "AM_COUNTRY = '" & Common.Parse(strCountry) & "' "
            strAdd &= "WHERE AM_COY_ID = '" & Common.Parse(strCoyID) & "' "
            strAdd &= "AND AM_ADDR_CODE = '" & Common.Parse(strCode) & "' "
            strAdd &= "AND AM_ADDR_TYPE = '" & Common.Parse(strType) & "' "
            If objDb.Execute(strAdd) Then
                modAddress = WheelMsgNum.Save
            Else
                modAddress = WheelMsgNum.NotSave
            End If
        End Function

        ' ai chu modified on 12/11/2005
        ' SR AS0030 - Prevent deletion of delivery address when have outstanding DO
        Public Function delAddress(ByVal dt As DataTable, ByVal strType As String, ByRef strNotDeleted As String) As Integer
            Dim strsql As String
            Dim i As Integer
            Dim strAryQuery(0) As String
            strNotDeleted = ""
            Dim blnDelete As Boolean

            For i = 0 To dt.Rows.Count - 1
                blnDelete = False
                ' only delivery address need to be checked whether there is/are outstanding DO
                If strType = "D" Then
                    strsql = "SELECT '*' FROM DO_MSTR, PO_MSTR "
                    strsql &= "WHERE DOM_PO_INDEX = POM_PO_INDEX "
                    strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql &= "AND DOM_D_ADDR_CODE = '" & Common.Parse(dt.Rows(i)("Code")) & "' "
                    strsql &= "AND DOM_DO_STATUS IN (1,2) "
                    If objDb.Exist(strsql) > 0 Then
                        'delAddress = -1
                        'Exit Function
                        strNotDeleted &= " - " & dt.Rows(i)("Code") & """&vbCrLf&"""
                        blnDelete = True
                    End If
                End If

                If Not blnDelete Then
                    strsql = "DELETE FROM ADDRESS_MSTR WHERE "
                    strsql &= "AM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(dt.Rows(i)("Code")) & "' "
                    strsql &= "AND AM_ADDR_TYPE = '" & strType & "' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    ' need to delete from USERS_DEFAULT_ADDR and USERS_LOCATION table also
                    strsql = "DELETE FROM USERS_DEFAULT_ADDR WHERE UDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql &= "AND UDA_ADDR_TYPE = '" & strType & "' "
                    strsql &= "AND UDA_ADDR_CODE = '" & Common.Parse(dt.Rows(i)("Code")) & "' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    If strType = "D" Then
                        strsql = "DELETE FROM USERS_LOCATION WHERE UL_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                        strsql &= "AND UL_ADDR_CODE = '" & Common.Parse(dt.Rows(i)("Code")) & "' "
                        Common.Insert2Ary(strAryQuery, strsql)
                    End If
                End If
            Next

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    delAddress = WheelMsgNum.Delete
                Else
                    delAddress = WheelMsgNum.NotDelete
                End If
            End If
        End Function

        Public Function getAddress(ByVal strCode As String, ByVal strType As String) As DataSet
            Dim objDb As New EAD.DBCom
            Dim strGet As String
            Dim dsAddress As New DataSet
            strGet = "SELECT * FROM ADDRESS_MSTR WHERE "
            strGet &= "AM_ADDR_CODE = '" & Common.Parse(strCode) & "' "
            strGet &= "AND AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strGet &= "AND AM_ADDR_TYPE = '" & Common.Parse(strType) & "' "
            dsAddress = objDb.FillDs(strGet)
            getAddress = dsAddress
        End Function
        Public Function addSoftware(ByVal strDesc As String)
            Dim strCoyId, strUserID, strSQL As String
            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            strSQL = "INSERT INTO COMPANY_SW (CSW_COY_ID, CSW_DESCRIPTION, CSW_ENT_BY, CSW_ENT_DT) VALUES('" & Common.Parse(strCoyId) & "','" & Common.Parse(strDesc) & "','" & strUserID & "'," & Common.ConvertDate(Now) & ")"
            If objDb.Execute(strSQL) Then
                addSoftware = WheelMsgNum.Save
            Else
                addSoftware = WheelMsgNum.NotSave
            End If
        End Function
        Public Function getSoftwareVendorDetail(ByVal CoyID As String) As DataSet
            'Dim objDb As New EAD.DBCom
            Dim strSql As String
            Dim dsSoftware As New DataSet

            strSql = "SELECT * FROM COMPANY_SW WHERE "
            strSql &= "CSW_COY_ID = '" & CoyID & "' "



            dsSoftware = objDb.FillDs(strSql)
            getSoftwareVendorDetail = dsSoftware
        End Function
        Public Function getSoftware(ByVal strDesc As String) As DataSet
            'Dim objDb As New EAD.DBCom
            Dim strSql As String
            Dim dsSoftware As New DataSet

            strSql = "SELECT * FROM COMPANY_SW WHERE "
            strSql &= "CSW_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strDesc <> "" Then
                strSql &= " AND CSW_DESCRIPTION LIKE '%" & strDesc & "%'"
            End If

            dsSoftware = objDb.FillDs(strSql)
            getSoftware = dsSoftware
        End Function
        Public Function modSoftware(ByVal strIndex As String, ByVal strDesc As String) As Integer
            Dim strUserID, strSQL As String
            strUserID = HttpContext.Current.Session("UserId")

            strSQL = "UPDATE COMPANY_SW SET "
            strSQL &= "CSW_DESCRIPTION = '" & Common.Parse(strDesc) & "', "
            strSQL &= "CSW_MOD_BY = '" & Common.Parse(strUserID) & "', "
            strSQL &= "CSW_MOD_DT = " & Common.ConvertDate(Now) & " "
            strSQL &= "WHERE CSW_INDEX = '" & Common.Parse(strIndex) & "' "
            If objDb.Execute(strSQL) Then
                modSoftware = WheelMsgNum.Save
            Else
                modSoftware = WheelMsgNum.NotSave
            End If
        End Function
        Public Function delSoftware(ByVal dt As DataTable) As Boolean
            Dim strsql As String
            Dim i As Integer
            Dim strAryQuery(0) As String
            Dim blnDelete As Boolean

            For i = 0 To dt.Rows.Count - 1
                strsql = "DELETE FROM COMPANY_SW WHERE "
                strsql &= "CSW_INDEX = '" & Common.Parse(dt.Rows(i)("Index")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    delSoftware = True
                Else
                    delSoftware = False
                End If
            End If
        End Function
        Public Function delQS(ByVal dt As DataTable) As Boolean
            Dim strsql As String
            Dim i As Integer
            Dim strAryQuery(0) As String
            Dim blnDelete As Boolean

            For i = 0 To dt.Rows.Count - 1
                strsql = "DELETE FROM COMPANY_QS WHERE "
                strsql &= "CQS_INDEX = '" & Common.Parse(dt.Rows(i)("Index")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    delQS = True
                Else
                    delQS = False
                End If
            End If
        End Function
        Public Function getQS() As DataSet
            'Dim objDb As New EAD.DBCom
            Dim strSql As String
            Dim dsQS As New DataSet

            strSql = "SELECT * FROM COMPANY_QS WHERE "
            strSql &= "CQS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            dsQS = objDb.FillDs(strSql)
            getQS = dsQS
        End Function
        Public Function updateComSetting(ByVal intFixed As Integer, ByVal intDisc As Integer, ByVal intContract As Integer)

            Dim strSql As String
            Dim strAryQuery(0) As String

            'Dim strCoyID As String

            'strCoyID = HttpContext.Current.Session("companyID")
            If objDb.Exist("SELECT '*' FROM COMPANY_SETTING WHERE CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CS_FLAG_NAME = 'Buy Fixed Price' AND CS_FLAG_TYPE ='BuyActivity'") > 0 Then
                strSql = "UPDATE COMPANY_SETTING SET CS_FLAG_VALUE =" & intFixed &
                   " WHERE CS_FLAG_NAME = 'Buy Fixed Price' AND CS_FLAG_TYPE = 'BuyActivity' AND CS_COY_ID= '" & HttpContext.Current.Session("CompanyId") & "'"
            Else
                strSql = "INSERT INTO COMPANY_SETTING(CS_COY_ID, CS_FLAG_NAME, CS_FLAG_VALUE, CS_FLAG_TYPE) VALUES ("
                strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                strSql &= "'Buy Fixed Price', "
                strSql &= "'" & intFixed & "', "
                strSql &= "'BuyActivity')"
            End If
            Common.Insert2Ary(strAryQuery, strSql)

            If objDb.Exist("SELECT '*' FROM COMPANY_SETTING WHERE CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CS_FLAG_NAME = 'Buy Discount Price' AND CS_FLAG_TYPE ='BuyActivity'") > 0 Then
                strSql = "UPDATE COMPANY_SETTING SET CS_FLAG_VALUE =" & intDisc &
                " where CS_FLAG_NAME = 'Buy Discount Price' and CS_FLAG_TYPE = 'BuyActivity' AND CS_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Else
                strSql = "INSERT INTO COMPANY_SETTING(CS_COY_ID, CS_FLAG_NAME, CS_FLAG_VALUE, CS_FLAG_TYPE) VALUES ("
                strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                strSql &= "'Buy Discount Price', "
                strSql &= "'" & intDisc & "', "
                strSql &= "'BuyActivity')"
            End If
            Common.Insert2Ary(strAryQuery, strSql)

            If objDb.Exist("SELECT '*' FROM COMPANY_SETTING WHERE CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CS_FLAG_NAME = 'Buy Contract Price' AND CS_FLAG_TYPE ='BuyActivity'") > 0 Then
                strSql = "UPDATE COMPANY_SETTING SET CS_FLAG_VALUE =" & intContract &
                " where CS_FLAG_NAME = 'Buy Contract Price' and CS_FLAG_TYPE = 'BuyActivity' AND CS_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Else
                strSql = "INSERT INTO COMPANY_SETTING(CS_COY_ID, CS_FLAG_NAME, CS_FLAG_VALUE, CS_FLAG_TYPE) VALUES ("
                strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                strSql &= " 'Buy Contract Price', "
                strSql &= "'" & intContract & "', "
                strSql &= "'BuyActivity')"
            End If
            Common.Insert2Ary(strAryQuery, strSql)

            ''strSql = "UPDATE COMPANY_SETTING SET CS_FLAG_VALUE =" & intFormat & _
            ''" where CS_FLAG_NAME = 'Free Format Item' and CS_FLAG_TYPE = 'BuyActivity'"
            ''Common.Insert2Ary(strAryQuery, strSql)

            objDb.BatchExecute(strAryQuery)
        End Function

        Public Function getFlag() As DataSet

            Dim objDb As New EAD.DBCom

            Dim strGet As String
            Dim dsFlag As New DataSet

            strGet = "SELECT CS_FLAG_NAME, CS_FLAG_VALUE FROM COMPANY_SETTING "
            strGet &= "where CS_FLAG_TYPE = 'BuyActivity' AND CS_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"

            dsFlag = objDb.FillDs(strGet)
            getFlag = dsFlag
        End Function

        Public Function updateUserDefault(ByVal userID As String, ByVal companyID As String, ByVal strType As String, ByVal strValue As String, ByVal strCode As String, ByVal strName As String, Optional ByVal strModule As String = "") As Boolean

            Dim strSql As String


            'Dim strCoyID As String

            'strCoyID = HttpContext.Current.Session("companyID")
            If strType = "B" Or strType = "D" Then  'Billing/Delivery Address
                If objDb.Exist("Select '*' From USERS_DEFAULT_ADDR Where UDA_COY_ID = '" & companyID & "' AND UDA_USER_ID = '" & userID & "' and UDA_ADDR_TYPE= '" & Common.Parse(strType) & "'") > 0 Then
                    strSql = "UPDATE USERS_DEFAULT_ADDR SET "
                    strSql &= "UDA_ADDR_CODE ='" & Common.Parse(strValue) & "' "
                    strSql &= "where UDA_COY_ID ='" & companyID & "' "
                    strSql &= "and UDA_USER_ID = '" & userID & "' "
                    strSql &= "and UDA_ADDR_TYPE= '" & Common.Parse(strType) & "' "
                Else
                    'insert
                    strSql = "insert into USERS_DEFAULT_ADDR VALUES ("
                    strSql &= "'" & companyID & "', "
                    strSql &= "'" & userID & "', "
                    strSql &= "'" & Common.Parse(strType) & "', "
                    strSql &= "'" & Common.Parse(strValue) & "') "
                End If

            ElseIf strType = "L" Then   'Location
                If objDb.Exist("Select '*' From user_default Where UD_COY_ID = '" & Common.Parse(companyID) & "' " _
                        & "AND UD_USER_ID = '" & Common.Parse(userID) & "' " _
                        & "AND UD_DEFAULT_TYPE= '" & Common.Parse(strCode) & "'") > 0 Then
                    strSql = "UPDATE user_default SET "
                    strSql &= "UD_DEFAULT_VALUE ='" & Common.Parse(strValue) & "', "
                    strSql &= "UD_MOD_BY='" & Common.Parse(userID) & "', UD_MOD_DATETIME=GETDATE() "
                    strSql &= "where UD_COY_ID ='" & Common.Parse(companyID) & "' "
                    strSql &= "and UD_USER_ID = '" & Common.Parse(userID) & "' "
                    strSql &= "and UD_DEFAULT_TYPE= '" & Common.Parse(strCode) & "'"
                Else
                    'insert
                    strSql = "insert into user_default VALUES ("
                    strSql &= "'" & Common.Parse(companyID) & "', "
                    strSql &= "'" & Common.Parse(userID) & "', "
                    strSql &= "'" & Common.Parse(strCode) & "', "
                    strSql &= "'" & Common.Parse(strValue) & "', "
                    strSql &= "'" & Common.Parse(userID) & "', "
                    strSql &= "GETDATE(), "
                    strSql &= "'" & Common.Parse(userID) & "', "
                    strSql &= "GETDATE()) "
                End If

            Else
                If objDb.Exist("Select '*' From USERS_DEFAULT_CUSTOMFIELDS Where UDC_COY_ID = '" & companyID & "' AND UDC_USER_ID = '" & userID & "' and UDC_FIELD_NO= '" & Common.Parse(strCode) & "' and UDC_TYPE = '" & Common.Parse(strModule) & "'") > 0 Then
                    strSql = "UPDATE USERS_DEFAULT_CUSTOMFIELDS "
                    strSql &= "SET UDC_FIELD_VALUE ='" & Common.Parse(strValue) & "' "
                    strSql &= "where UDC_COY_ID ='" & companyID & "' "
                    strSql &= "and UDC_USER_ID = '" & userID & "' "
                    strSql &= "and UDC_FIELD_NO = '" & Common.Parse(strCode) & "' "                'and UDC_FIELD_NAME = '" & Common.Parse(strName) & "'"
                    strSql &= "and UDC_TYPE = '" & Common.Parse(strModule) & "'"
                Else
                    strSql = "insert into USERS_DEFAULT_CUSTOMFIELDS VALUES ("
                    strSql &= "'" & companyID & "', "
                    strSql &= "'" & userID & "', "
                    strSql &= "'" & Common.Parse(strCode) & "', "
                    strSql &= "'" & Common.Parse(strName) & "', "
                    strSql &= "'" & Common.Parse(strValue) & "', "
                    strSql &= "'" & Common.Parse(strModule) & "') "
                    'objDb.Execute(strSql)

                End If

                'Public Function moddept(ByVal strDeptCode As String, ByVal strDeptName As String) As String
                '            Dim strSql_moddept As String
                '            strSql_moddept = "update COMPANY_DEPT_MSTR set CDM_DEPT_NAME ='" & Common.Parse(strDeptName) & _
                '            "' where CDM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & _
                '            "' and CDM_DEPT_CODE = '" & Common.Parse(strDeptCode) & "'"
                '            If objDb.Execute(strSql_moddept) Then
                '                moddept = RecordSave
                '            Else
                '                moddept = RecordNotSave
                '            End If
                '        End Function
                'Common.Insert2Ary(strAryQuery, strSql)
            End If

            Return objDb.Execute(strSql)

        End Function

        'Public Function getCustomFieldList() As DataSet
        '    Dim strSql As String
        '    Dim dsCustom As New DataSet()
        '    strSql = "SELECT DISTINCT CF_FIELD_NO, CF_FIELD_NAME "
        '    strSql &= "FROM CUSTOM_FIELDS "
        '    strSql &= "WHERE CF_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
        '    dsCustom = objDb.FillDs(strSql)
        '    getCustomFieldList = dsCustom
        'End Function
        Public Function getCustomFieldListing(ByVal strName As String, ByVal CFmodule As String) As DataView
            Dim strSql As String
            Dim dv As DataView

            strSql = "SELECT * FROM CUSTOM_FIELDS WHERE CF_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
            If strName = "" And CFmodule = "" Then
            ElseIf strName <> "" And CFmodule <> "" Then
                strSql &= " AND CF_FIELD_NAME ='" & Common.Parse(strName) & "' AND CF_MODULE='" & CFmodule & "'"
            ElseIf strName <> "" Then
                strSql &= " AND CF_FIELD_NAME ='" & Common.Parse(strName) & "'"
            ElseIf CFmodule <> "" Then
                strSql &= " AND CF_MODULE='" & CFmodule & "'"
            End If
            dv = objDb.GetView(strSql)
            Return dv

        End Function
        Public Function getCustomFieldName(ByVal CFmodule As String) As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT DISTINCT CF_FIELD_NO, CF_FIELD_NAME "
            strSql &= "FROM CUSTOM_FIELDS "
            strSql &= "WHERE CF_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If CFmodule <> "" Then
                strSql = strSql & " AND CF_Module" & Common.ParseSQL(CFmodule)
            End If
            strSql = strSql & " ORDER BY CF_FIELD_NAME"
            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function getCustomFieldPR(ByVal strName As String, Optional ByVal strType As String = "", Optional ByVal strPRIndex As String = "") As DataView
            '    'Dim myData As New Wheel.Data.WheelDataProvider()
            '    'Dim drvCodeMstr As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT DISTINCT PCM_FIELD_NO AS CF_FIELD_NO, PCM_FIELD_NAME AS CF_FIELD_NAME "
            strSql &= "FROM PR_CUSTOM_FIELD_MSTR "
            strSql &= " WHERE PCM_PR_INDEX = '" & strPRIndex & "' "

            If strName <> "" Then
                strSql = strSql & " AND PCM_FIELD_NAME " & Common.ParseSQL(strName)
            End If

            If strType <> "" Then
                strSql = strSql & " AND PCM_TYPE " & Common.ParseSQL(strType)
            Else
                strSql = strSql & " AND PCM_TYPE = 'PR'"
            End If

            strSql = strSql & " ORDER BY PCM_FIELD_NAME"
            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function getCustomField(ByVal companyID As String, ByVal strName As String, Optional ByVal strType As String = "") As DataView
            '    'Dim myData As New Wheel.Data.WheelDataProvider()
            '    'Dim drvCodeMstr As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT DISTINCT CF_FIELD_NO, CF_FIELD_NAME "
            strSql &= "FROM CUSTOM_FIELDS "
            strSql &= "WHERE CF_COY_ID = '" & companyID & "' "

            If strName <> "" Then
                strSql = strSql & " AND CF_FIELD_NAME " & Common.ParseSQL(strName)
            End If

            If strType <> "" Then
                strSql = strSql & " AND CF_MODULE " & Common.ParseSQL(strType)
            Else
                strSql = strSql & " AND CF_MODULE = 'PR'"
            End If
            strSql = strSql & " ORDER BY CF_FIELD_NAME"
            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function getCustomFieldDS(ByVal strName As String, Optional ByVal strType As String = "") As DataSet
            Dim strSql As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom
            strSql = "SELECT DISTINCT CF_FIELD_NO, CF_FIELD_NAME "
            strSql &= "FROM CUSTOM_FIELDS "
            strSql &= "WHERE CF_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strName <> "" Then
                strSql = strSql & " AND CF_FIELD_NAME " & Common.ParseSQL(strName)
            End If

            If strType <> "" Then
                strSql = strSql & " AND CF_MODULE " & Common.ParseSQL(strType)
            Else
                strSql = strSql & " AND CF_MODULE = 'PR'"
            End If
            strSql = strSql & " ORDER BY CF_FIELD_NAME"
            ds = objDB.FillDs(strSql)
            Return ds
        End Function

        Public Function getexrate(ByVal strcode As String, ByVal strDesc As String) As DataSet
            Dim strsql As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom
            strsql = "SELECT CE_CURRENCY_CODE,CODE_DESC,CE_RATE from COMPANY_EXCHANGERATE LEFT JOIN CODE_MSTR ON CE_CURRENCY_CODE = CODE_ABBR WHERE CE_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            '//---meilai---//
            If strcode <> "" Then
                strsql &= "AND CE_CURRENCY_CODE " & Common.ParseSQL(strcode)
            End If
            If strDesc <> "" Then
                strsql &= "AND CODE_DESC " & Common.ParseSQL(strDesc)
            End If
            '//----meilai---//
            'If strcode <> "" Then
            '        strsql &= "AND CE_CURRENCY_CODE LIKE '%" & strcode & "%' "
            '    End If
            '    If strDesc <> "" Then
            '        strsql &= "AND CODE_DESC LIKE '%" & strDesc & "%' "
            '    End If
            ' 'strsql &= "where  = "
            ds = objDB.FillDs(strsql)
            Return ds
        End Function

        Public Function modexrate(ByVal strCode As String, ByVal dblRate As Double) As String
            Dim strsql As String
            Dim objDB As New EAD.DBCom

            If objDB.Exist("SELECT '*' FROM COMPANY_EXCHANGERATE WHERE CE_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CE_CURRENCY_CODE ='" & Common.Parse(strCode) & "'") > 0 Then
                strsql = "UPDATE COMPANY_EXCHANGERATE set "
                strsql &= "CE_RATE = " & dblRate & ", "
                strsql &= "CE_MOD_BY = '" & HttpContext.Current.Session("UserID") & "', "
                strsql &= "CE_MOD_DATETIME = " & Common.ConvertDate(Now) & " "
                strsql &= "WHERE CE_CURRENCY_CODE = '" & Common.Parse(strCode) & "' "
                strsql &= "AND CE_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            Else
                strsql = "INSERT INTO COMPANY_EXCHANGERATE(CE_COY_ID,CE_CURRENCY_CODE, CE_RATE, CE_ENT_BY, CE_ENT_DATETIME) VALUES ("
                strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                strsql &= "'" & Common.Parse(strCode) & "', "
                strsql &= "'" & dblRate & "', "
                strsql &= "'" & HttpContext.Current.Session("UserID") & "', "
                strsql &= "" & Common.ConvertDate(Now) & ")"
            End If


            'objDB.Execute(strsql)
            modexrate = strsql
        End Function

        Public Function RetExRate(ByVal strCode As String, Optional ByVal dateFrom As String = "", Optional ByVal dateTo As String = "") As DataSet
            Dim strsql As String
            Dim objDB As New EAD.DBCom
            Dim ds As New DataSet

            strsql = "SELECT CE_CURRENCY_INDEX, CE_COY_ID, CE_CURRENCY_CODE, " &
                    "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_ABBR = CE_CURRENCY_CODE) AS CE_CURRENCY_NAME, " &
                    "CE_RATE, CE_VALID_FROM, CE_VALID_TO FROM COMPANY_EXCHANGERATE " &
                    "WHERE CE_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CE_DELETED = 'N' "

            If strCode <> "" Then
                strsql &= "AND CE_CURRENCY_CODE = '" & Common.Parse(strCode) & "' "
            End If

            If dateFrom <> "" Then
                strsql &= "AND CE_VALID_FROM >= " & Common.ConvertDate(dateFrom) & " "
            End If

            If dateTo <> "" Then
                strsql &= "AND CE_VALID_TO <= " & Common.ConvertDate(dateTo & " 23:59:59.000") & " "
            End If

            strsql &= "ORDER BY CE_CURRENCY_CODE, CE_VALID_FROM "

            ds = objDB.FillDs(strsql)
            RetExRate = ds
        End Function

        Public Function AddBatchExRate(ByVal dsRate As DataSet) As Boolean
            Dim strsql As String
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet
            Dim i As Integer
            Dim strAryQuery(0) As String

            For i = 0 To dsRate.Tables(0).Rows.Count - 1
                strsql = "SELECT CE_CURRENCY_INDEX FROM COMPANY_EXCHANGERATE WHERE CE_DELETED = 'Y' AND CE_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CE_CURRENCY_CODE = '" & dsRate.Tables(0).Rows(i)("CE_CURRENCY_CODE") & "'"
                ds = objDB.FillDs(strsql)

                If ds.Tables(0).Rows.Count > 0 Then
                    strsql = "UPDATE COMPANY_EXCHANGERATE SET " &
                            "CE_CURRENCY_CODE = '" & dsRate.Tables(0).Rows(i)("CE_CURRENCY_CODE") & "', " &
                            "CE_RATE = '" & dsRate.Tables(0).Rows(i)("CE_RATE") & "', " &
                            "CE_VALID_FROM = " & Common.ConvertDate(dsRate.Tables(0).Rows(i)("CE_VALID_FROM")) & ", " &
                            "CE_VALID_TO = " & Common.ConvertDate(dsRate.Tables(0).Rows(i)("CE_VALID_TO") & " 23:59:59.000") & ", " &
                            "CE_ENT_BY = '" & HttpContext.Current.Session("UserID") & "', " &
                            "CE_ENT_DATETIME = " & Common.ConvertDate(Now) & ", " &
                            "CE_MOD_BY = NULL, " &
                            "CE_MOD_DATETIME = NULL, " &
                            "CE_DELETED = 'N' " &
                            "WHERE CE_CURRENCY_INDEX = '" & ds.Tables(0).Rows(0)("CE_CURRENCY_INDEX") & "' "

                    Common.Insert2Ary(strAryQuery, strsql)
                Else
                    strsql = "INSERT INTO COMPANY_EXCHANGERATE(CE_COY_ID,CE_CURRENCY_CODE, CE_RATE, CE_ENT_BY, CE_ENT_DATETIME, CE_VALID_FROM, CE_VALID_TO, CE_DELETED) VALUES (" &
                            "'" & HttpContext.Current.Session("CompanyId") & "', " &
                            "'" & dsRate.Tables(0).Rows(i)("CE_CURRENCY_CODE") & "', " &
                            "'" & dsRate.Tables(0).Rows(i)("CE_RATE") & "', " &
                            "'" & HttpContext.Current.Session("UserID") & "', " &
                            Common.ConvertDate(Now) & ", " &
                            Common.ConvertDate(dsRate.Tables(0).Rows(i)("CE_VALID_FROM")) & ", " &
                            Common.ConvertDate(dsRate.Tables(0).Rows(i)("CE_VALID_TO") & " 23:59:59.000") & ", " &
                            "'N')"

                    Common.Insert2Ary(strAryQuery, strsql)
                End If
            Next

            If objDB.BatchExecute(strAryQuery) Then
                AddBatchExRate = True
            Else
                AddBatchExRate = False
            End If

        End Function

        Public Function AddExRate(ByVal strCode As String, ByVal dblRate As Double, ByVal dateFrom As String, ByVal dateTo As String) As String
            Dim strsql As String
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet

            strsql = "SELECT CE_CURRENCY_INDEX FROM COMPANY_EXCHANGERATE WHERE CE_DELETED = 'Y' AND CE_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CE_CURRENCY_CODE = '" & Common.Parse(strCode) & "'"
            ds = objDB.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                strsql = "UPDATE COMPANY_EXCHANGERATE SET "
                strsql &= "CE_CURRENCY_CODE = '" & Common.Parse(strCode) & "', "
                strsql &= "CE_RATE = '" & Common.Parse(dblRate) & "', "
                strsql &= "CE_VALID_FROM = " & Common.ConvertDate(dateFrom) & ", "
                strsql &= "CE_VALID_TO = " & Common.ConvertDate(dateTo & " 23:59:59.000") & ", "
                strsql &= "CE_ENT_BY = '" & HttpContext.Current.Session("UserID") & "', "
                strsql &= "CE_ENT_DATETIME = " & Common.ConvertDate(Now) & ", "
                strsql &= "CE_MOD_BY = NULL, "
                strsql &= "CE_MOD_DATETIME = NULL, "
                strsql &= "CE_DELETED = 'N' "
                strsql &= "WHERE CE_CURRENCY_INDEX = '" & ds.Tables(0).Rows(0)("CE_CURRENCY_INDEX") & "' "
            Else
                strsql = "INSERT INTO COMPANY_EXCHANGERATE(CE_COY_ID,CE_CURRENCY_CODE, CE_RATE, CE_ENT_BY, CE_ENT_DATETIME, CE_VALID_FROM, CE_VALID_TO, CE_DELETED) VALUES ("
                strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                strsql &= "'" & Common.Parse(strCode) & "', "
                strsql &= "'" & Common.Parse(dblRate) & "', "
                strsql &= "'" & HttpContext.Current.Session("UserID") & "', "
                strsql &= Common.ConvertDate(Now) & ", "
                strsql &= Common.ConvertDate(dateFrom) & ", "
                strsql &= Common.ConvertDate(dateTo & " 23:59:59.000") & ", "
                strsql &= "'N')"
            End If

            If objDB.Execute(strsql) Then
                AddExRate = WheelMsgNum.Save
            Else
                AddExRate = WheelMsgNum.NotSave
            End If

        End Function

        Public Function UpdateExRate(ByVal strIndex As String, ByVal strCode As String, ByVal dblRate As Double, ByVal dateFrom As String, ByVal dateTo As String) As String
            Dim strsql As String
            Dim objDB As New EAD.DBCom

            strsql = "UPDATE COMPANY_EXCHANGERATE SET "
            strsql &= "CE_CURRENCY_CODE = '" & Common.Parse(strCode) & "', "
            strsql &= "CE_RATE = '" & Common.Parse(dblRate) & "', "
            strsql &= "CE_VALID_FROM = " & Common.ConvertDate(dateFrom) & ", "
            strsql &= "CE_VALID_TO = " & Common.ConvertDate(dateTo & " 23:59:59.000") & ", "
            strsql &= "CE_MOD_BY = '" & HttpContext.Current.Session("UserID") & "', "
            strsql &= "CE_MOD_DATETIME = " & Common.ConvertDate(Now) & " "
            strsql &= "WHERE CE_CURRENCY_INDEX = '" & Common.Parse(strIndex) & "' "

            objDB.Execute(strsql)

            If objDB.Execute(strsql) Then
                UpdateExRate = WheelMsgNum.Save
            Else
                UpdateExRate = WheelMsgNum.NotSave
            End If

        End Function

        Public Function delExRate(ByVal dtCurr As DataTable) As Integer

            Dim strAry(0) As String
            Dim strSql, strUserID As String
            Dim drCurr As DataRow
            strUserID = HttpContext.Current.Session("UserId")

            For Each drCurr In dtCurr.Rows
                strSql = "UPDATE COMPANY_EXCHANGERATE SET CE_DELETED='Y' WHERE CE_CURRENCY_INDEX = " & drCurr("Curr_Index")
                Common.Insert2Ary(strAry, strSql)
            Next

            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        Public Function updateItemCode(ByVal strPCode As String, ByVal strBCode As String, ByVal strCategoryCode As String, ByVal strGLCode As String, ByVal strTaxCode As String) As String
            Dim objDb As New EAD.DBCom
            Dim strSql As String

            If objDb.Exist("Select * From COMPANY_B_ITEM_CODE Where CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CBC_PRODUCT_CODE = '" & Common.Parse(strPCode) & "'") > 0 Then
                strSql = "UPDATE COMPANY_B_ITEM_CODE "
                ' ai chu modified on 06/09/2005 - add 2 more fields 
                strSql &= "SET CBC_B_ITEM_CODE = '" & Common.Parse(strBCode) & "', "
                strSql &= "CBC_B_CATEGORY_CODE = '" & Common.Parse(strCategoryCode) & "', "
                'Michelle (16/6/2010) - To include Tax Code 
                'strSql &= "CBC_B_GL_CODE = '" & Common.Parse(strGLCode) & "' "
                strSql &= "CBC_B_GL_CODE = '" & Common.Parse(strGLCode) & "', "
                strSql &= "CBC_B_TAX_CODE = '" & Common.Parse(strTaxCode) & "' "
                strSql &= "where CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strSql &= "and CBC_PRODUCT_CODE = '" & Common.Parse(strPCode) & "';"
            Else
                strSql = "insert into COMPANY_B_ITEM_CODE (CBC_B_COY_ID, CBC_PRODUCT_CODE, CBC_B_ITEM_CODE, "
                strSql &= "CBC_B_CATEGORY_CODE, CBC_B_GL_CODE, CBC_B_TAX_CODE) VALUES ("
                strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                strSql &= "'" & Common.Parse(strPCode) & "', "
                strSql &= "'" & Common.Parse(strBCode) & "', "
                strSql &= "'" & Common.Parse(strCategoryCode) & "', "
                'Michelle (16/6/2010) - To include Tax Code 
                'strSql &= "'" & Common.Parse(strGLCode) & "'); "
                strSql &= "'" & Common.Parse(strGLCode) & "', '" & Common.Parse(strTaxCode) & "'); "
            End If

            updateItemCode = strSql
        End Function

        Public Function getvenparam()
            Dim objDb As New EAD.DBCom
            Dim strGet As String
            Dim dsParam As New DataSet

            strGet = "SELECT CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE FROM COMPANY_PARAM WHERE CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "
            'strGet = strGet & "Select CM_PAYMENT_TERM,CM_PAYMENT_METHOD,CM_PWD_DURATION from COMPANY_MSTR where CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "
            'strGet &= "Select CS_FLAG_NAME,CS_FLAG_VALUE from COMPANY_SETTING where CS_FLAG_TYPE= 'CoyParam'"
            'dsParam = objDb.FillDs(strGet)


            dsParam = objDb.FillDs(strGet)
            getvenparam = dsParam



        End Function


        Public Function updatevenparam(ByVal htPageAccess As Hashtable)

            Dim myEnumerator As IDictionaryEnumerator = htPageAccess.GetEnumerator()
            Dim objDb As New EAD.DBCom
            Dim strUpdate As String
            Dim strValue As String
            Dim strPrefix As String
            Dim strAryQuery(0) As String
            Dim strsql As String

            While myEnumerator.MoveNext()
                If myEnumerator.Key = "" Then
                    Exit While
                End If
                strValue = myEnumerator.Value
                ' strUpdate = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE = '" & Common.Parse(strValue) & "' "
                If Right(myEnumerator.Key, 6).Trim.ToUpper = "PREFIX" Then
                    strPrefix = Left(myEnumerator.Key, Len(myEnumerator.Key) - 6).Trim
                    If objDb.Exist("SELECT '*' FROM COMPANY_PARAM WHERE CP_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CP_PARAM_TYPE = '" & strPrefix & "' AND CP_PARAM_NAME = 'Prefix'") > 0 Then
                        strUpdate = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE = '" & Common.Parse(strValue) & "' "
                        strUpdate &= "WHERE CP_PARAM_TYPE = '" & strPrefix & "' "
                        strUpdate &= "AND CP_PARAM_NAME = 'Prefix' and CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                    Else
                        strUpdate = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_VALUE, CP_PARAM_TYPE, CP_PARAM_NAME) VALUES("
                        strUpdate &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                        strUpdate &= "'" & Common.Parse(strValue) & "', "
                        strUpdate &= "'" & strPrefix & "', "
                        strUpdate &= "'Prefix')"
                    End If
                Else
                    strPrefix = Left(myEnumerator.Key, Len(myEnumerator.Key) - 10).Trim
                    If objDb.Exist("SELECT '*' FROM COMPANY_PARAM WHERE CP_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CP_PARAM_TYPE = '" & strPrefix & "' AND CP_PARAM_NAME = 'Last Used No'") > 0 Then
                        strUpdate = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE = '" & Common.Parse(strValue) & "' "
                        strUpdate &= "WHERE CP_PARAM_TYPE = '" & strPrefix & "' "
                        strUpdate &= "AND CP_PARAM_NAME = 'Last Used No' and CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                    Else
                        strUpdate = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_VALUE, CP_PARAM_TYPE, CP_PARAM_NAME) VALUES ("
                        strUpdate &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                        strUpdate &= "'" & Common.Parse(strValue) & "', "
                        strUpdate &= "'" & strPrefix & "', "
                        strUpdate &= "'Last Used No')"
                    End If
                End If
                Common.Insert2Ary(strAryQuery, strUpdate)
            End While

            'Dim strsql As String
            'strsql = "update COMPANY_PARAM set "
            'strsql &= "CP_PARAM_NAME ='" & Common.Parse(paramname) '" & _ 
            'strsql &= "CP_PARAM_VALUE ='" & Common.Parse(paramvalue) '"


            'Dim strSql As String
            'strSql = "update COMPANY_PARAM set CP_PARAM_NAME='" & Common.Parse(strparamname) & "', "
            'strSql &= "CP_PARAM_VALUE='" & Common.Parse(strparamvalue) & "', CP_PARAM_TYPE='" & Common.Parse(strparamtype) & "' "
            'strSql &= "WHERE CP_COY_ID= '" & "supplier" & "'"
            'Common.Insert2Ary(strAryQuery, strSql)

            'strSql = "update COMPANY_SETTING set CS_FLAG_VALUE ='" & Common.Parse(intFreeForm) & "' where CS_FLAG_NAME = 'Allow Free Form Billing Address' "
            'Common.Insert2Ary(strAryQuery, strSql)

            'strSql = "update COMPANY_SETTING set CS_FLAG_VALUE ='" & Common.Parse(intConsolidation) & "' where CS_FLAG_NAME = 'Consolidation Required' "
            'Common.Insert2Ary(strAryQuery, strSql)

            'strSql = "update COMPANY_SETTING set CS_FLAG_VALUE ='" & Common.Parse(intLevelsRec) & "' where CS_FLAG_NAME = '2 Level Receiving' "
            'Common.Insert2Ary(strAryQuery, strSql)

            objDb.BatchExecute(strAryQuery)
        End Function

        Public Function isApprovedVendor(ByVal strBCoyId As String, ByVal strSCoyId As String) As Boolean
            Dim strsql As String
            strsql = "SELECT CM_COY_ID FROM COMPANY_VENDOR, COMPANY_MSTR "
            strsql &= "WHERE CV_S_COY_ID = CM_COY_ID "
            strsql &= "AND CV_B_COY_ID = '" & strBCoyId & "' "
            strsql &= "AND CV_S_COY_ID = '" & strSCoyId & "' "
            If objDb.Exist(strsql) > 0 Then
                isApprovedVendor = True
            Else
                isApprovedVendor = False
            End If
        End Function

        Public Function withGRNCtrl(ByVal companyID As String) As Boolean
            Dim strsql As String
            strsql = "SELECT CM_GRN_CONTROL FROM COMPANY_MSTR "
            strsql &= "WHERE CM_COY_ID = '" & companyID & "' AND CM_GRN_CONTROL = 'Y' "
            If objDb.Exist(strsql) > 0 Then
                withGRNCtrl = True
            Else
                withGRNCtrl = False
            End If
        End Function
        Public Function searchvendor(ByVal strType As String, ByVal strVendorID As String, ByVal strVendorName As String, Optional ByVal blnSortVCode As Boolean = False, Optional ByVal blnGST As Boolean = False, Optional ByVal isFFPO As Boolean = False) As DataSet
            Dim strsql_svendor As String
            Dim dssvendor As DataSet
            Dim strTemp As String

            If strType = "AV" Then    'Vendor already at approved vendor list
                'Jules 2014.07.14 GST enhancement         
                If Not blnGST Then
                    strsql_svendor = "select v.*,CM_COY_ID,m.CM_COY_NAME, m.CM_BUSINESS_REG_NO, m.CM_TAX_REG_NO " &
                            " from Company_Vendor v, Company_Mstr m " &
                            " where v.CV_S_COY_ID=m.CM_COY_ID and v.CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND CM_STATUS = 'A' "
                ElseIf isFFPO Then
                    'Zulham Aug 27, 2014
                    'For FFPO, this function is called from TypeAhead.aspx. For FFPO, the blnGST is always True to skip the above condition and isFFPO = True.
                    strsql_svendor = "SELECT CM_COY_NAME,CM_COY_ID,CM_TAX_CALC_BY, CV_Payment_Term, CV_Payment_Method, CM_CURRENCY_CODE, CV_BILLING_METHOD FROM COMPANY_MSTR "
                    strsql_svendor &= "INNER JOIN COMPANY_VENDOR ON CM_COY_ID = CV_S_COY_ID AND CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql_svendor &= "WHERE CM_DELETED <> 'Y' AND CM_STATUS = 'A' " 'End
                Else
                    strsql_svendor = "select v.*,CM_COY_ID,m.CM_COY_NAME, m.CM_BUSINESS_REG_NO, " &
                                    "(SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_ID=v.CV_S_COY_ID) AS CM_TAX_REG_NO" &
                                    " from Company_Vendor v, Company_Mstr m " &
                                    " where v.CV_S_COY_ID=m.CM_COY_ID and v.CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND CM_STATUS = 'A' "
                End If
                'end.

            Else    'Vendor in Hub that not in the approved vendor list
                'filter own company
                'Rashid To view user in System Maintinance 
                'strsql_svendor = "SELECT CM_COY_ID, CM_COY_NAME, CM_BUSINESS_REG_NO, CM_TAX_REG_NO, CM_PAYMENT_TERM AS CV_PAYMENT_TERM, CM_PAYMENT_METHOD AS CV_PAYMENT_METHOD, '' AS CV_BILLING_METHOD, '' AS CV_GRN_CTRL_TERM, '' AS CV_GST_RATE, '' AS CV_GST_TAX_CODE " &
                '" FROM COMPANY_MSTR WHERE CM_STATUS = 'A' AND CM_DELETED = 'N' AND CM_COY_TYPE IN ('VENDOR','BOTH') AND CM_COY_ID NOT IN (SELECT CV_S_COY_ID FROM COMPANY_VENDOR " &
                '" WHERE CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "') AND CM_COY_ID <> '" & HttpContext.Current.Session("CompanyId") & "'"

                strsql_svendor = "SELECT CM_COY_ID, CM_COY_NAME, CM_BUSINESS_REG_NO, CM_TAX_REG_NO, CM_PAYMENT_TERM AS CV_PAYMENT_TERM, CM_PAYMENT_METHOD AS CV_PAYMENT_METHOD, '' AS CV_BILLING_METHOD, '' AS CV_GRN_CTRL_TERM, '' AS CV_GST_RATE, '' AS CV_GST_TAX_CODE " &
                " FROM COMPANY_MSTR WHERE CM_STATUS = 'A' AND CM_DELETED = 'N' AND CM_COY_TYPE NOT IN ('BUYER','other','NONE')  AND CM_COY_ID NOT IN (SELECT CV_S_COY_ID FROM COMPANY_VENDOR " &
                " WHERE CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "') AND CM_COY_ID <> '" & HttpContext.Current.Session("CompanyId") & "'"
            End If


            If strVendorID <> "" Then
                strTemp = Common.BuildWildCard(strVendorID)
                strsql_svendor = strsql_svendor & " AND CM_COY_ID" & Common.ParseSQL(strTemp)
            End If

            If strVendorName <> "" Then
                strTemp = Common.BuildWildCard(strVendorName)
                strsql_svendor = strsql_svendor & " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            If blnSortVCode Then
                strsql_svendor = strsql_svendor & " order by CM_COY_ID"
            Else
                strsql_svendor = strsql_svendor & " order by CM_COY_NAME"
            End If
            dssvendor = objDb.FillDs(strsql_svendor)
            searchvendor = dssvendor
        End Function
        Public Function searchapprvendorforBIM() As DataSet
            Dim strsql_svendor As String
            Dim dssvendor As DataSet

            'strsql_svendor = "select '' as CM_COY_ID, '---Select---' as CM_COY_NAME union "
            strsql_svendor &= "select CM_COY_ID,m.CM_COY_NAME " &
                 " from Company_Vendor v, Company_Mstr m " &
                 " where v.CV_S_COY_ID=m.CM_COY_ID and v.CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND m.CM_DELETED <> 'Y' order by CM_COY_NAME "

            'strsql_svendor &= "select distinct CM_COY_ID, m.CM_COY_NAME " & _
            '     " from Company_Vendor v, Company_Mstr m " & _
            '     " where v.CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND (CM_COY_TYPE='VENDOR' OR CM_COY_TYPE='BOTH') order by CM_COY_NAME "

            dssvendor = objDb.FillDs(strsql_svendor)
            searchapprvendorforBIM = dssvendor
        End Function
        Public Function searchvendorfor() As DataSet
            Dim strsql_svendor As String
            Dim dssvendor As DataSet

            strsql_svendor &= "select CM_COY_ID,m.CM_COY_NAME " &
                 " from Company_Mstr m " &
                 " where m.CM_DELETED <> 'Y' AND (CM_COY_TYPE='VENDOR' OR CM_COY_TYPE='BOTH') order by CM_COY_NAME "

            dssvendor = objDb.FillDs(strsql_svendor)
            searchvendorfor = dssvendor
        End Function
        Public Function delvendor(ByVal strVendorID As String) As Integer
            Dim strSql_delvendor As String
            ' Dim strSql_remaindept As String
            '//need to check whether got BCM before deletion
            Dim strAryQuery(0) As String
            strSql_delvendor = "delete FROM COMPANY_VENDOR where CV_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' and CV_S_COY_ID= '" & strVendorID & "'"
            Common.Insert2Ary(strAryQuery, strSql_delvendor)
            'strSql_delvendor = "delete from RFQ_VEN_DISTR_LIST_DETAIL where " & _
            '" RCDLD_V_Coy_ID='" & strVendorID & "' and RVDLD_LIST_INDEX in " & _
            '"(select RVDLM_List_Index from RFQ_VEN_DISTR_LIST_mstr where RVDLM_Coy_Id='" & HttpContext.Current.Session("CompanyId") & "')"
            Common.Insert2Ary(strAryQuery, strSql_delvendor)

            If objDb.BatchExecute(strAryQuery) Then
                delvendor = WheelMsgNum.Delete
            Else
                delvendor = WheelMsgNum.NotDelete
            End If
        End Function


        Public Function updvendor(ByVal strVendorID As String, ByVal strPTerm As String, ByVal strPMeth As String, ByVal strInv As String, ByVal intEDD As Integer,
        ByVal strType As String, Optional ByVal strGRN As String = "", Optional ByVal strGSTRate As String = "", Optional ByVal strGSTTaxCode As String = "") As String
            Dim strUpdate As String
            Dim strCoyID As String

            'Michelle (30/7/2010) - To include the default EDD
            If strType = "AV" Then
                'If strmode = "AV" Then
                strUpdate = "UPDATE COMPANY_VENDOR set CV_PAYMENT_TERM ='" & strPTerm & "', CV_PAYMENT_METHOD='" & strPMeth
                strUpdate &= "', CV_BILLING_METHOD='" & strInv & "', CV_EDD = " & intEDD & ", CV_GRN_CTRL_TERM='" & strGRN
                strUpdate &= "', CV_MOD_BY='" & HttpContext.Current.Session("UserId") & "', CV_MOD_DT = NOW()"
                strUpdate &= ", CV_GST_RATE='" & strGSTRate & "', CV_GST_TAX_CODE='" & strGSTTaxCode & "'"
                strUpdate &= " where CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' and CV_S_COY_ID='" & strVendorID & "'"

            Else
                ' 'Insert Into COMPANY_Vendor(CV_B_COY_ID,CV_S_COY_ID,CV_PAYMENT_TERM, CV_PAYMENT_METHOD,CV_BILLING_METHOD)values ('demo','testing','90 days','cash','DO')

                strUpdate = "INSERT INTO COMPANY_Vendor(CV_B_COY_ID,CV_S_COY_ID,CV_GST_RATE,"
                strUpdate &= "CV_GST_TAX_CODE,"
                strUpdate &= "CV_PAYMENT_TERM, CV_PAYMENT_METHOD,CV_BILLING_METHOD, CV_EDD, CV_GRN_CTRL_TERM,CV_MOD_BY,CV_MOD_DT)values ("
                strUpdate &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                strUpdate &= "'" & Common.Parse(strVendorID) & "', "
                strUpdate &= "'" & Common.Parse(strGSTRate) & "', "
                strUpdate &= "'" & Common.Parse(strGSTTaxCode) & "', "
                strUpdate &= "'" & Common.Parse(strPTerm) & "', "
                strUpdate &= "'" & Common.Parse(strPMeth) & "', "
                strUpdate &= "'" & Common.Parse(strInv) & "', "
                strUpdate &= "'" & Common.Parse(intEDD) & "', "
                strUpdate &= "'" & Common.Parse(strGRN) & "', "
                strUpdate &= "'" & HttpContext.Current.Session("UserId") & "', "
                strUpdate &= "NOW()) "
            End If

            updvendor = strUpdate
            'If objDb.Execute(strUpdate) Then
            '    updvendor = WheelMsgNum.Save
            'Else
            '    updvendor = WheelMsgNum.NotSave
            'End If
        End Function

        Public Function GetAssetGroupInfo(ByRef strAGroupCode As String, ByRef strAGroupDesc As String, ByRef strCodeType As String, ByRef strAGroupStatus As String) As DataSet
            Dim strSQL As String
            Dim ds1 As DataSet

            'Get asset group / sub group code & description
            strSQL = "SELECT AG_GROUP, AG_GROUP_DESC, AG_GROUP_TYPE, AG_STATUS " _
                & "FROM asset_group WHERE AG_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If strAGroupCode <> "" Then
                strSQL &= "AND AG_GROUP LIKE '%" & Common.Parse(strAGroupCode) & "%' "
            End If
            If strAGroupDesc <> "" Then
                strSQL &= "AND AG_GROUP_DESC LIKE '%" & Common.Parse(strAGroupDesc) & "%' "
            End If
            If strCodeType <> "" Then
                strSQL &= "AND AG_GROUP_TYPE LIKE '%" & Common.Parse(strCodeType) & "%' "
            End If
            If strAGroupStatus <> "" Then
                strSQL &= "AND AG_STATUS LIKE '%" & Common.Parse(strAGroupStatus) & "%' "
            End If

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                strAGroupCode = ds1.Tables(0).Rows(0).Item("AG_GROUP")
                strAGroupDesc = ds1.Tables(0).Rows(0).Item("AG_GROUP_DESC")
                strCodeType = ds1.Tables(0).Rows(0).Item("AG_GROUP_TYPE")
                strAGroupStatus = ds1.Tables(0).Rows(0).Item("AG_STATUS")
            Else
                strAGroupCode = "Group/Sub Group Code"
                strAGroupDesc = "Group/Sub Group Description"
                strCodeType = "Code Type"
                strAGroupStatus = "Status"
            End If

            Return ds1
        End Function

        Public Function ModAssetGroup(ByVal strAGroupCode As String, ByVal strAGroupDesc As String, ByVal strCodeType As String, ByRef strAGStatus As String) As String
            Dim strSQL, strPRExist, strPOExist As String
            Dim strOStatus, strOGrpTyp As String
            Dim ds As DataSet

            strSQL = "SELECT AG_STATUS, AG_GROUP_TYPE FROM ASSET_GROUP " _
                     & "WHERE AG_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                     & "AND AG_GROUP='" & Common.Parse(strAGroupCode) & "'"
            ds = objDb.FillDs(strSQL)
            strOStatus = ds.Tables(0).Rows(0).Item("AG_STATUS")
            strOGrpTyp = ds.Tables(0).Rows(0).Item("AG_GROUP_TYPE")

            If strOStatus <> strAGStatus Or strOGrpTyp <> strCodeType Then
                If strAGStatus = "I" Then
                    strPRExist = "SELECT '*' " _
                            & "FROM PR_DETAILS " _
                            & "LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " _
                            & "WHERE PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                            & "AND PRD_ASSET_GROUP = '" & Common.Parse(strAGroupCode) & "' AND PRM_PR_STATUS <> '9' "
                    strPOExist = "SELECT '*' " _
                            & "FROM PO_DETAILS " _
                            & "LEFT JOIN PO_MSTR ON POD_COY_ID = POM_B_COY_ID AND POM_PO_NO = POD_PO_NO " _
                            & "WHERE POD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                            & "AND POD_ASSET_GROUP = '" & Common.Parse(strAGroupCode) & "' AND POM_PO_STATUS <> '12' "

                    If objDb.Exist(strPRExist) > 0 Or objDb.Exist(strPOExist) > 0 Then
                        Return -99
                    End If
                End If

                strSQL = "SELECT * FROM asset_group " _
                         & "WHERE AG_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                         & "AND AG_GROUP='" & Common.Parse(strAGroupCode) & "' " _
                         & "AND AG_GROUP_DESC='" & Common.Parse(strAGroupDesc) & "' " _
                         & "AND AG_STATUS = '" & Common.Parse(strAGStatus) & "' " _
                         & "AND AG_GROUP_TYPE = '" & Common.Parse(strCodeType) & "'"
            Else
                strSQL = "SELECT * FROM asset_group " _
                         & "WHERE AG_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                         & "AND AG_GROUP='" & Common.Parse(strAGroupCode) & "' " _
                         & "AND AG_GROUP_DESC='" & Common.Parse(strAGroupDesc) & "'"
            End If

            If objDb.Exist(strSQL) = 0 Then ' record not exists
                strSQL = "UPDATE asset_group SET "
                If Common.Parse(strAGroupCode) = "" Then
                    strSQL &= "AG_GROUP=NULL, "
                Else
                    strSQL &= "AG_GROUP='" & Common.Parse(strAGroupCode) & "',"
                End If
                If Common.Parse(strAGroupDesc) = "" Then
                    strSQL &= "AG_GROUP_DESC=NULL, "
                Else
                    strSQL &= "AG_GROUP_DESC='" & Common.Parse(strAGroupDesc) & "',"
                End If
                If Common.Parse(strCodeType) = "" Then
                    strSQL &= "AG_GROUP_TYPE=NULL, "
                Else
                    strSQL &= "AG_GROUP_TYPE='" & Common.Parse(strCodeType) & "',"
                End If
                If Common.Parse(strAGStatus) = "" Then
                    strSQL &= "AG_STATUS=NULL, "
                Else
                    strSQL &= "AG_STATUS='" & Common.Parse(strAGStatus) & "',"
                End If
                strSQL &= "AG_MOD_BY='" & Common.Parse(HttpContext.Current.Session("UserID")) & "', AG_MOD_DATETIME=" & Common.ConvertDate(Now()) & " " _
                        & "WHERE AG_GROUP='" & Common.Parse(strAGroupCode) & "'"
                If objDb.Execute(strSQL) Then
                    ModAssetGroup = WheelMsgNum.Save
                Else
                    ModAssetGroup = WheelMsgNum.NotSave
                End If

            Else
                ModAssetGroup = WheelMsgNum.Duplicate
            End If

        End Function

        Public Function DeleteAssetGroup(ByVal dtAGroup As DataTable) As Integer
            Dim strAry(0) As String
            Dim strSQL, strPRExist, strPOExist, strInvExist As String
            Dim drAG As DataRow

            For Each drAG In dtAGroup.Rows
                strInvExist = "SELECT * FROM invoice_mstr " _
                        & "INNER JOIN invoice_details " _
                        & "ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " _
                        & "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND id_asset_group = '" & Common.Parse(drAG("AGCode")) & "' " _
                        & "OR id_asset_sub_group = '" & Common.Parse(drAG("AGCode")) & "' "
                strPRExist = "SELECT '*' " _
                        & "FROM PR_DETAILS " _
                        & "LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " _
                        & "WHERE PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND PRD_ASSET_GROUP = '" & Common.Parse(drAG("AGCode")) & "' AND PRM_PR_STATUS <> '9' "
                strPOExist = "SELECT '*' " _
                        & "FROM PO_DETAILS " _
                        & "LEFT JOIN PO_MSTR ON POD_COY_ID = POM_B_COY_ID AND POM_PO_NO = POD_PO_NO " _
                        & "WHERE POD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND POD_ASSET_GROUP = '" & Common.Parse(drAG("AGCode")) & "' AND POM_PO_STATUS <> '12' "
                If objDb.Exist(strInvExist) > 0 Or objDb.Exist(strPRExist) > 0 Or objDb.Exist(strPOExist) > 0 Then
                    Return -99
                End If

                strSQL = "SELECT * FROM asset_group WHERE AG_GROUP='" & Common.Parse(drAG("AGCode")) & "'"

                If objDb.Exist(strSQL) <> 0 Then
                    strSQL = "DELETE FROM asset_group WHERE AG_GROUP='" & Common.Parse(drAG("AGCode")) & "'"
                    Common.Insert2Ary(strAry, strSQL)
                End If
            Next
            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        Public Function AddAssetGroup(ByVal strAGroupCode As String, ByVal strAGroupDesc As String, ByVal strCodeType As String, ByRef strAGStatus As String) As String
            Dim strSQL As String
            strSQL = "SELECT * FROM asset_group WHERE AG_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND AG_GROUP='" & Common.Parse(strAGroupCode) & "'"

            If objDb.Exist(strSQL) = 0 Then ' record not exists
                strSQL = "INSERT INTO asset_group (AG_COY_ID,AG_GROUP,AG_GROUP_DESC,AG_GROUP_TYPE,AG_STATUS,AG_ENT_BY,AG_ENT_DATETIME,AG_MOD_BY,AG_MOD_DATETIME) "
                strSQL &= "VALUES ('" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "',"
                If Common.Parse(strAGroupCode) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strAGroupCode) & "',"
                End If
                If Common.Parse(strAGroupDesc) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strAGroupDesc) & "',"
                End If
                If Common.Parse(strCodeType) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strCodeType) & "',"
                End If
                If Common.Parse(strAGStatus) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strAGStatus) & "',"
                End If

                strSQL &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ",'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ")"
                If objDb.Execute(strSQL) Then
                    AddAssetGroup = WheelMsgNum.Save
                Else
                    AddAssetGroup = WheelMsgNum.NotSave
                End If

            Else
                AddAssetGroup = WheelMsgNum.Duplicate
            End If

        End Function

        Public Function SearchAssetGroupInfo(ByRef strGroupCode As String, ByRef strGroupDesc As String, ByRef chkAsset As Boolean, ByRef chkSub As Boolean, ByRef chkActive As Boolean, ByRef chkInactive As Boolean)
            Dim strSQL As String
            Dim ds As DataSet
            Dim ds1 As DataSet
            Dim strCodeType As String
            Dim strStatus As String

            'Get branch code & branch description
            strSQL = "SELECT AG_GROUP, AG_GROUP_DESC, AG_GROUP_TYPE, AG_STATUS " _
                & "FROM asset_group WHERE AG_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If strGroupCode <> "" Then
                strSQL &= "AND AG_GROUP LIKE '%" & Common.Parse(strGroupCode) & "%' "
            End If
            If strGroupDesc <> "" Then
                strSQL &= "AND AG_GROUP_DESC LIKE '%" & Common.Parse(strGroupDesc) & "%' "
            End If
            If chkAsset = True Or chkSub = True Then
                If chkSub = True And chkAsset = True Then
                    strSQL &= "AND (AG_GROUP_TYPE ='A' OR AG_GROUP_TYPE = 'S') "
                ElseIf chkAsset = True And chkSub = False Then
                    strSQL &= "AND AG_GROUP_TYPE ='A' "
                ElseIf chkSub = True And chkAsset = False Then
                    strSQL &= "AND AG_GROUP_TYPE ='S' "
                End If

            End If

            If chkActive = True Or chkInactive = True Then
                If chkActive = True And chkInactive = True Then
                    strSQL &= "AND (AG_STATUS ='A' OR AG_STATUS = 'I') "
                ElseIf chkActive = True And chkInactive = False Then
                    strSQL &= "AND AG_STATUS ='A' "
                ElseIf chkInactive = True And chkActive = False Then
                    strSQL &= "AND AG_STATUS ='I' "
                End If

            End If


            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                strGroupCode = ds1.Tables(0).Rows(0).Item("AG_GROUP")
                strGroupDesc = ds1.Tables(0).Rows(0).Item("AG_GROUP_DESC")
                strCodeType = ds1.Tables(0).Rows(0).Item("AG_GROUP_TYPE")
                strStatus = ds1.Tables(0).Rows(0).Item("AG_STATUS")
            Else
                strGroupCode = "Group/Sub Group Code"
                strGroupDesc = "Group/Sub Group Description"
                strCodeType = "Code Type"
                strStatus = "Status"
            End If

            ds = Nothing
            'ds1 = Nothing
            Return ds1
        End Function

        Public Function PopulateAssetGroup() As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet

            strSql = "SELECT AG_GROUP, AG_GROUP_DESC, AG_GROUP_TYPE, AG_STATUS " _
                & "FROM asset_group WHERE AG_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' "

            dsGroup = objDb.FillDs(strSql)
            PopulateAssetGroup = dsGroup

        End Function

        Public Function CheckPending(ByVal strFieldToCheck As String, ByVal strIPP As String) As Boolean
            Dim strSQL As String

            If strIPP = "IPP" Then
                strSQL = "SELECT * FROM invoice_mstr WHERE im_s_coy_name='" & strFieldToCheck & "'" _
                        & "AND im_invoice_status = 10"
            ElseIf strIPP = "CC" Then 'argument: cc_cc_code
                'strSQL = "SELECT * FROM cost_alloc_detail WHERE cad_cc_code='" & strFieldToCheck & "'"
                strSQL = "SELECT * FROM invoice_mstr " _
                        & "INNER JOIN invoice_details " _
                        & "ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " _
                        & "LEFT JOIN invoice_details_alloc " _
                        & "ON im_invoice_index = ida_invoice_index " _
                        & "LEFT JOIN cost_alloc_detail ON cad_cc_code = id_cost_center " _
                        & "WHERE im_invoice_status = 10 AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND id_cost_center = ida_cost_center " _
                        & "AND ida_cost_center = '" & strFieldToCheck & "' "
            ElseIf strIPP = "CB" Then
                strSQL = "SELECT * FROM cost_alloc_detail WHERE cad_branch_code = cb_cc_code " _
                        & "SELECT * FROM invoice_mstr " _
                        & "INNER JOIN invoice_details " _
                        & "ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " _
                        & "LEFT JOIN invoice_details_alloc " _
                        & "ON im_invoice_index = ida_invoice_index " _
                        & "WHERE im_invoice_status = 10 " _
                        & "AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND ida_branch_code = '" & strFieldToCheck & "' "
            ElseIf strIPP = "AG" Then
                strSQL = "SELECT * FROM invoice_mstr " _
                        & "INNER JOIN invoice_details " _
                        & "ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " _
                        & "WHERE im_invoice_status = 10 AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND id_asset_group = '" & strFieldToCheck & "' " _
                        & "OR id_asset_sub_group = '" & strFieldToCheck & "' "
            End If

            If objDb.Exist(strSQL) <> 0 Then
                Return False
            End If
            Return 0
        End Function

        Public Function getDelTerm(ByVal strCode As String, ByVal strName As String) As DataSet
            'Dim objDb As New EAD.DBCom
            Dim strSql As String
            Dim ds As New DataSet

            strSql = "SELECT * FROM COMPANY_DELIVERY_TERM WHERE "
            strSql &= "CDT_DELETED = 'N' AND CDT_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strCode <> "" Then
                strSql &= " AND CDT_DEL_CODE LIKE '%" & Common.Parse(strCode) & "%'"
            End If

            If strName <> "" Then
                strSql &= " AND CDT_DEL_NAME LIKE '%" & Common.Parse(strName) & "%'"
            End If

            ds = objDb.FillDs(strSql)
            getDelTerm = ds
        End Function

        Public Function addDelTerm(ByVal strDelCode As String, ByVal strDelName As String, ByVal intFactor As Decimal, ByVal strOversea As String)
            Dim strCoyId, strUserID, strSQL As String
            Dim ds As New DataSet
            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            strSQL = "SELECT '*' fROM COMPANY_DELIVERY_TERM WHERE "
            strSQL &= "CDT_DEL_CODE = '" & Common.Parse(strDelCode) & "' AND CDT_COY_ID = '" & Common.Parse(strCoyId) & "' AND CDT_DELETED ='N'"

            If objDb.Exist(strSQL) > 0 Then
                addDelTerm = WheelMsgNum.Duplicate
                Exit Function
            End If

            strSQL = "SELECT CDT_DEL_INDEX fROM COMPANY_DELIVERY_TERM WHERE "
            strSQL &= "CDT_DEL_CODE = '" & Common.Parse(strDelCode) & "' AND CDT_COY_ID = '" & Common.Parse(strCoyId) & "'  AND CDT_DELETED ='Y'"

            ds = objDb.FillDs(strSQL)
            If ds.Tables(0).Rows.Count > 0 Then
                strSQL = "UPDATE COMPANY_DELIVERY_TERM SET "
                strSQL &= "CDT_DEL_CODE = '" & Common.Parse(strDelCode) & "', "
                strSQL &= "CDT_DEL_NAME = '" & Common.Parse(strDelName) & "', "
                strSQL &= "CDT_DEL_GRNFACTOR = '" & Common.Parse(intFactor) & "', "
                strSQL &= "CDT_DEL_OVERSEA = '" & strOversea & "', "
                strSQL &= "CDT_DELETED = 'N', "
                strSQL &= "CDT_ENT_BY = '" & Common.Parse(strUserID) & "', "
                strSQL &= "CDT_ENT_DT = " & Common.ConvertDate(Now) & ", "
                strSQL &= "CDT_MOD_BY = NULL, "
                strSQL &= "CDT_MOD_DT = NULL "
                strSQL &= "WHERE CDT_DEL_INDEX = '" & ds.Tables(0).Rows(0)("CDT_DEL_INDEX") & "' "
            Else
                strSQL = "INSERT INTO COMPANY_DELIVERY_TERM (CDT_COY_ID, CDT_DEL_CODE, CDT_DEL_NAME, CDT_DEL_GRNFACTOR, CDT_DEL_OVERSEA, CDT_ENT_BY, CDT_ENT_DT, CDT_DELETED) " &
                        "VALUES('" & Common.Parse(strCoyId) & "','" & Common.Parse(strDelCode) & "','" & Common.Parse(strDelName) & "','" & Common.Parse(intFactor) & "','" & strOversea & "','" & strUserID & "'," & Common.ConvertDate(Now) & ",'N')"
            End If

            If objDb.Execute(strSQL) Then
                addDelTerm = WheelMsgNum.Save
            Else
                addDelTerm = WheelMsgNum.NotSave
            End If
        End Function

        Public Function modDelTerm(ByVal strIndex As String, ByVal strDelCode As String, ByVal strDelName As String, ByVal intFactor As Decimal, ByVal strOversea As String) As Integer
            Dim strUserID, strCoyID, strSQL As String
            Dim ds As New DataSet
            strUserID = HttpContext.Current.Session("UserId")
            strCoyID = HttpContext.Current.Session("CompanyId")

            strSQL = "SELECT '*' fROM COMPANY_DELIVERY_TERM WHERE "
            strSQL &= "CDT_DEL_CODE = '" & Common.Parse(strDelCode) & "' AND CDT_COY_ID = '" & Common.Parse(strCoyID) & "' "
            strSQL &= "AND CDT_DEL_INDEX <> '" & strIndex & "' "

            ds = objDb.FillDs(strSQL)
            If ds.Tables(0).Rows.Count > 0 Then
                modDelTerm = WheelMsgNum.Duplicate
                Exit Function
            End If

            strSQL = "UPDATE COMPANY_DELIVERY_TERM SET "
            strSQL &= "CDT_DEL_CODE = '" & Common.Parse(strDelCode) & "', "
            strSQL &= "CDT_DEL_NAME = '" & Common.Parse(strDelName) & "', "
            strSQL &= "CDT_DEL_GRNFACTOR = '" & Common.Parse(intFactor) & "', "
            strSQL &= "CDT_DEL_OVERSEA = '" & strOversea & "', "
            strSQL &= "CDT_MOD_BY = '" & Common.Parse(strUserID) & "', "
            strSQL &= "CDT_MOD_DT = " & Common.ConvertDate(Now) & " "
            strSQL &= "WHERE CDT_DEL_INDEX = '" & Common.Parse(strIndex) & "' "
            If objDb.Execute(strSQL) Then
                modDelTerm = WheelMsgNum.Save
            Else
                modDelTerm = WheelMsgNum.NotSave
            End If
        End Function

        Public Function delDelTerm(ByVal dtDelTerm As DataTable) As Integer

            Dim strAry(0) As String
            Dim strSql, strUserID, strCoyId As String
            Dim drDelTerm As DataRow
            'strUserID = HttpContext.Current.Session("UserId")
            strCoyId = HttpContext.Current.Session("CompanyId")

            For Each drDelTerm In dtDelTerm.Rows
                'Check RFQ
                strSql = "SELECT '*' FROM RFQ_MSTR WHERE RM_DEL_CODE = '" & Common.Parse(drDelTerm("Del_Code")) & "' AND RM_COY_ID = '" & strCoyId & "'"
                If objDb.Exist(strSql) <> 0 Then
                    Return -99
                End If

                'Check PO
                strSql = "SELECT '*' FROM PO_MSTR WHERE POM_DEL_CODE = '" & Common.Parse(drDelTerm("Del_Code")) & "' AND POM_B_COY_ID = '" & strCoyId & "'"
                If objDb.Exist(strSql) <> 0 Then
                    Return -99
                End If

                'Check PR
                strSql = "SELECT '*' FROM PR_DETAILS WHERE PRD_COY_ID = '" & strCoyId & "' AND PRD_DEL_CODE = '" & Common.Parse(drDelTerm("Del_Code")) & "'"
                If objDb.Exist(strSql) <> 0 Then
                    Return -99
                End If

                'Check Product
                strSql = "SELECT '*' FROM PRODUCT_MSTR, PIM_VENDOR WHERE PM_PRODUCT_INDEX = PV_PRODUCT_INDEX AND PM_S_COY_ID = '" & strCoyId & "' AND PV_DELIVERY_TERM = '" & Common.Parse(drDelTerm("Del_Code")) & "'"
                If objDb.Exist(strSql) <> 0 Then
                    Return -99
                End If

                'Check Quotation
                strSql = "SELECT * FROM RFQ_MSTR, RFQ_VENDOR_MSTR, RFQ_REPLIES_DETAIL WHERE RM_RFQ_ID = RVM_RFQ_ID And RVM_RFQ_ID = RRD_RFQ_ID AND RVM_V_COMPANY_ID = RRD_V_COY_ID AND RM_COY_ID = '" & strCoyId & "' AND RRD_DEL_CODE = '" & Common.Parse(drDelTerm("Del_Code")) & "'"
                If objDb.Exist(strSql) <> 0 Then
                    Return -99
                End If

                strSql = "UPDATE COMPANY_DELIVERY_TERM SET CDT_DELETED='Y' WHERE CDT_DEL_INDEX = " & drDelTerm("Del_Index")
                Common.Insert2Ary(strAry, strSql)
            Next

            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        Public Function getPackType(ByVal strCode As String, ByVal strName As String) As DataSet
            'Dim objDb As New EAD.DBCom
            Dim strSql As String
            Dim ds As New DataSet

            strSql = "SELECT * FROM COMPANY_PACKING_TYPE WHERE "
            strSql &= "CPT_DELETED = 'N' AND CPT_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strCode <> "" Then
                strSql &= " AND CPT_PACK_CODE LIKE '%" & Common.Parse(strCode) & "%'"
            End If

            If strName <> "" Then
                strSql &= " AND CPT_PACK_NAME LIKE '%" & Common.Parse(strName) & "%'"
            End If

            ds = objDb.FillDs(strSql)
            getPackType = ds
        End Function

        Public Function addPackType(ByVal strPackCode As String, ByVal strPackName As String)
            Dim strCoyId, strUserID, strSQL As String
            Dim ds As New DataSet
            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            strSQL = "SELECT '*' FROM COMPANY_PACKING_TYPE WHERE "
            strSQL &= "CPT_PACK_CODE = '" & Common.Parse(strPackCode) & "' AND CPT_COY_ID = '" & strCoyId & "' AND CPT_DELETED='N' "

            If objDb.Exist(strSQL) > 0 Then
                addPackType = WheelMsgNum.Duplicate
                Exit Function
            End If

            strSQL = "SELECT CPT_PACK_INDEX FROM COMPANY_PACKING_TYPE WHERE "
            strSQL &= "CPT_PACK_CODE = '" & Common.Parse(strPackCode) & "' AND CPT_COY_ID = '" & strCoyId & "' AND CPT_DELETED='Y' "

            ds = objDb.FillDs(strSQL)
            If ds.Tables(0).Rows.Count > 0 Then
                strSQL = "UPDATE COMPANY_PACKING_TYPE SET "
                strSQL &= "CPT_PACK_CODE = '" & Common.Parse(strPackCode) & "', "
                strSQL &= "CPT_PACK_NAME = '" & Common.Parse(strPackName) & "', "
                strSQL &= "CPT_DELETED = 'N', "
                strSQL &= "CPT_ENT_BY = '" & Common.Parse(strUserID) & "', "
                strSQL &= "CPT_ENT_DT = " & Common.ConvertDate(Now) & ", "
                strSQL &= "CPT_MOD_BY = NULL, "
                strSQL &= "CPT_MOD_DT = NULL "
                strSQL &= "WHERE CPT_PACK_INDEX = '" & ds.Tables(0).Rows(0)("CPT_PACK_INDEX") & "' "
            Else
                strSQL = "INSERT INTO COMPANY_PACKING_TYPE (CPT_COY_ID, CPT_PACK_CODE, CPT_PACK_NAME, CPT_ENT_BY, CPT_ENT_DT, CPT_DELETED) VALUES('" & Common.Parse(strCoyId) & "','" & Common.Parse(strPackCode) & "','" & Common.Parse(strPackName) & "','" & strUserID & "'," & Common.ConvertDate(Now) & ",'N')"
            End If

            If objDb.Execute(strSQL) Then
                addPackType = WheelMsgNum.Save
            Else
                addPackType = WheelMsgNum.NotSave
            End If
        End Function

        Public Function modPackType(ByVal strIndex As String, ByVal strPackCode As String, ByVal strPackName As String) As Integer
            Dim strUserID, strCoyID, strSQL As String
            strUserID = HttpContext.Current.Session("UserId")
            strCoyID = HttpContext.Current.Session("CompanyId")
            Dim ds As New DataSet

            strSQL = "SELECT '*' FROM COMPANY_PACKING_TYPE WHERE "
            strSQL &= "CPT_PACK_CODE = '" & Common.Parse(strPackCode) & "' AND CPT_COY_ID = '" & strCoyID & "' "
            strSQL &= "AND CPT_PACK_INDEX <> '" & strIndex & "' "

            ds = objDb.FillDs(strSQL)
            If ds.Tables(0).Rows.Count > 0 Then
                modPackType = WheelMsgNum.Duplicate
                Exit Function
            End If

            strSQL = "UPDATE COMPANY_PACKING_TYPE SET "
            strSQL &= "CPT_PACK_CODE = '" & Common.Parse(strPackCode) & "', "
            strSQL &= "CPT_PACK_NAME = '" & Common.Parse(strPackName) & "', "
            strSQL &= "CPT_MOD_BY = '" & Common.Parse(strUserID) & "', "
            strSQL &= "CPT_MOD_DT = " & Common.ConvertDate(Now) & " "
            strSQL &= "WHERE CPT_PACK_INDEX = '" & Common.Parse(strIndex) & "' "
            If objDb.Execute(strSQL) Then
                modPackType = WheelMsgNum.Save
            Else
                modPackType = WheelMsgNum.NotSave
            End If
        End Function

        Public Function delPackType(ByVal dtPack As DataTable) As Integer

            Dim strAry(0) As String
            Dim strSql, strUserID As String
            Dim drPack As DataRow
            strUserID = HttpContext.Current.Session("UserId")

            For Each drPack In dtPack.Rows
                strSql = "SELECT '*' FROM PRODUCT_MSTR WHERE PM_PACKING_TYPE='" & Common.Parse(drPack("Pack_Code")) & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                If objDb.Exist(strSql) <> 0 Then
                    Return -99
                End If

                strSql = "UPDATE COMPANY_PACKING_TYPE SET CPT_DELETED='Y' WHERE CPT_PACK_INDEX = " & drPack("Pack_Index")
                Common.Insert2Ary(strAry, strSql)
            Next

            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        Public Function getUsrWithSection() As DataView

            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "SELECT DISTINCT CONCAT(CS_SEC_CODE, ' : ' , CS_SEC_NAME) AS SECTION, CS_SEC_INDEX FROM company_section WHERE CS_DELETED = 'N' AND CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function getSection(ByVal strCode As String, ByVal strName As String) As DataSet
            Dim strSql As String
            Dim ds As New DataSet

            strSql = "SELECT * FROM COMPANY_SECTION WHERE "
            strSql &= "CS_DELETED = 'N' AND CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strCode <> "" Then
                strSql &= " AND CS_SEC_CODE LIKE '%" & Common.Parse(strCode) & "%'"
            End If

            If strName <> "" Then
                strSql &= " AND CS_SEC_NAME LIKE '%" & Common.Parse(strName) & "%'"
            End If

            ds = objDb.FillDs(strSql)
            getSection = ds
        End Function

        Public Function AddSecCode(ByVal strSecCode As String, ByVal strSecName As String, ByVal strMode As String, ByVal strIndex As String)
            Dim strCoyId, strUserID, strSQL As String
            Dim ds As New DataSet
            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            strSQL = "SELECT CS_DELETED, CS_SEC_INDEX FROM COMPANY_SECTION WHERE "
            strSQL &= "CS_SEC_CODE = '" & Common.Parse(strSecCode) & "' AND CS_COY_ID = '" & strCoyId & "' AND CS_DELETED = 'N' "

            If strMode = "mod" Then
                strSQL &= "AND CS_SEC_INDEX <> '" & Common.Parse(strIndex) & "' "
            End If

            If objDb.Exist(strSQL) > 0 Then
                Return -99
            Else
                If strMode = "add" Then
                    strSQL = "SELECT CS_DELETED, CS_SEC_INDEX FROM COMPANY_SECTION WHERE "
                    strSQL &= "CS_SEC_CODE = '" & Common.Parse(strSecCode) & "' AND CS_COY_ID = '" & strCoyId & "' AND CS_DELETED = 'Y' "

                    ds = objDb.FillDs(strSQL)
                    If ds.Tables(0).Rows.Count > 0 Then
                        strSQL = "UPDATE COMPANY_SECTION SET "
                        strSQL &= "CS_SEC_CODE = '" & Common.Parse(strSecCode) & "', "
                        strSQL &= "CS_SEC_NAME = '" & Common.Parse(strSecName) & "', "
                        strSQL &= "CS_DELETED = 'N', "
                        strSQL &= "CS_ENT_BY = '" & Common.Parse(strUserID) & "', "
                        strSQL &= "CS_ENT_DT = " & Common.ConvertDate(Now) & ", "
                        strSQL &= "CS_MOD_BY = NULL, "
                        strSQL &= "CS_MOD_DT = NULL "
                        strSQL &= "WHERE CS_SEC_INDEX = '" & ds.Tables(0).Rows(0)("CS_SEC_INDEX") & "' "
                    Else
                        strSQL = "INSERT INTO COMPANY_SECTION (CS_COY_ID, CS_SEC_CODE, CS_SEC_NAME, CS_ENT_BY, CS_ENT_DT, CS_DELETED) VALUES('" & Common.Parse(strCoyId) & "','" & Common.Parse(strSecCode) & "','" & Common.Parse(strSecName) & "','" & strUserID & "'," & Common.ConvertDate(Now) & ",'N')"
                    End If
                Else
                    strSQL = "UPDATE COMPANY_SECTION SET "
                    strSQL &= "CS_SEC_CODE = '" & Common.Parse(strSecCode) & "', "
                    strSQL &= "CS_SEC_NAME = '" & Common.Parse(strSecName) & "', "
                    strSQL &= "CS_MOD_BY = '" & Common.Parse(strUserID) & "', "
                    strSQL &= "CS_MOD_DT = " & Common.ConvertDate(Now) & " "
                    strSQL &= "WHERE CS_SEC_INDEX = '" & Common.Parse(strIndex) & "' "
                End If
            End If

            If objDb.Execute(strSQL) Then
                AddSecCode = WheelMsgNum.Save
            Else
                AddSecCode = WheelMsgNum.NotSave
            End If

        End Function

        Public Function delSecCode(ByVal dtSecCode As DataTable) As Integer

            Dim strAry(0) As String
            Dim strSql, strUserID As String
            Dim drSecCode As DataRow
            strUserID = HttpContext.Current.Session("UserId")

            For Each drSecCode In dtSecCode.Rows
                strSql = "SELECT '*' FROM COMPANY_SECTION_BUYER WHERE CSB_SECTION_INDEX=" & Common.Parse(drSecCode("Sec_Index"))
                If objDb.Exist(strSql) <> 0 Then
                    Return -99
                End If

                strSql = "SELECT '*' FROM INVENTORY_REQUISITION_MSTR WHERE IRM_IR_SECTION='" & Common.Parse(drSecCode("Sec_Code")) & "' AND IRM_IR_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                If objDb.Exist(strSql) <> 0 Then
                    Return -99
                End If

                strSql = "UPDATE COMPANY_SECTION SET CS_DELETED='Y' WHERE CS_SEC_INDEX = " & drSecCode("Sec_Index")
                Common.Insert2Ary(strAry, strSql)
            Next

            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        Public Function getIQCTestType() As DataView

            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "SELECT DISTINCT CPA_PARAM_LABEL FROM COMPANY_PARAM_ADDITIONAL WHERE CPA_PARAM_TYPE = 'IQC' AND CPA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function getIQCSetting() As DataSet
            Dim strCoyId, strUserID, strSQL As String
            Dim ds As New DataSet
            strCoyId = HttpContext.Current.Session("CompanyId")

            strSQL = "SELECT CPA_INDEX, CPA_COY_ID, CPA_PARAM_LABEL, CPA_PARAM_PREFIX, CPA_PARAM_VALUE, CPA_PARAM_ATTACHMENT " &
                    "FROM COMPANY_PARAM_ADDITIONAL WHERE CPA_COY_ID = '" & strCoyId & "' AND CPA_PARAM_TYPE = 'IQC' "

            ds = objDb.FillDs(strSQL)
            getIQCSetting = ds
        End Function

        Public Function getAllCurrWithRate() As DataSet
            Dim strCoyId, strSQL As String
            Dim ds As New DataSet
            strCoyId = HttpContext.Current.Session("CompanyId")

            strSQL = "SELECT CODE_ABBR, CODE_DESC, " &
                    "IFNULL((SELECT CE_RATE FROM COMPANY_EXCHANGERATE WHERE CE_CURRENCY_CODE = CODE_ABBR AND CE_COY_ID = '" & strCoyId & "' AND CE_DELETED = 'N' " &
                    "ORDER BY CE_VALID_TO DESC  LIMIT 1),0) AS RATE " &
                    "FROM CODE_MSTR WHERE CODE_CATEGORY = 'CU' " &
                    "ORDER BY CODE_ABBR"

            ds = objDb.FillDs(strSQL)
            getAllCurrWithRate = ds
        End Function

        Public Function getGLCodeOnlyTypeAhead(Optional ByVal compid As String = "", Optional ByVal strUserInput As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            If compid <> "" Then
                If strUserInput = "*" Then
                    strsql = "SELECT CBG_B_GL_CODE " _
                    & "FROM COMPANY_B_GL_CODE " _
                    & "WHERE CBG_B_COY_ID ='" & Common.Parse(compid) & "' AND CBG_STATUS = 'A' "
                Else
                    'Zulham 13022015 Case 8317
                    strsql = "SELECT CBG_B_GL_CODE " _
                  & "FROM COMPANY_B_GL_CODE " _
                  & "WHERE CBG_B_COY_ID ='" & Common.Parse(compid) & "' AND CBG_STATUS = 'A' and CBG_B_GL_CODE like '%" & strUserInput & "%'" 'Jules 2014.03.19 - Capex Enhancement
                End If

            Else
                If strUserInput = "*" Then
                    strsql = "SELECT CBG_B_GL_CODE " _
                                      & "FROM COMPANY_B_GL_CODE " _
                                      & "WHERE CBG_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND CBG_STATUS = 'A' "
                Else
                    strsql = "SELECT CBG_B_GL_CODE " _
                  & "FROM COMPANY_B_GL_CODE " _
                  & "WHERE CBG_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND CBG_STATUS = 'A' and CBG_B_GL_CODE like '%" & Common.Parse(strUserInput) & "%'"
                End If
            End If


            ds = objDb.FillDs(strsql)
            getGLCodeOnlyTypeAhead = ds
        End Function

        ''Jules 2014.03.18 - For Capex Enhancement.
        Public Function InsertAuditGLCodeCategoryCode(ByVal strGLCode As String, ByVal strCategoryCode As String, ByVal strAction As String)
            Dim objDb As New EAD.DBCom
            Dim strSql As String = "", pQuery(0) As String

            strSql = "INSERT INTO AU_COMPANY_B_GL_CODE_CATEGORY_CODE "
            strSql &= "(AU_CBGC_B_COY_ID,AU_CBGC_B_GL_CODE,AU_CBGC_B_CATEGORY_CODE,AU_USER,AU_DATE,AU_ACTION) "
            strSql &= "VALUES ("
            strSql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
            strSql &= "'" & Common.Parse(strGLCode) & "', "
            strSql &= "'" & Common.Parse(strCategoryCode) & "', "
            strSql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
            strSql &= Common.ConvertDate(Now()) & ", "
            strSql &= "'" & strAction & "')"
            Common.Insert2Ary(pQuery, strSql)
            objDb.BatchExecute(pQuery)
        End Function

        Public Function getTaxCodeInfo(ByVal strCode As String, Optional ByVal strCat As String = "eProcure", Optional ByVal strCoyId As String = "") As DataSet
            Dim strSql As String
            Dim ds As New DataSet

            'strSql = "SELECT TM_TAX_CODE, TM_TAX_DESC, TM_TAX_TYPE, TM_TAX_RATE, TM_COUNTRY_CODE " & _
            '        "FROM TAX_MSTR " & _
            '        "WHERE TM_DELETED <> 'Y' AND TM_CATEGORY = '" & strCat & "' "
            If strCat = "IPP" Then
                Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
                If strDefIPPCompID = "" Then
                    strDefIPPCompID = strCoyId
                End If
                'Zulham 30112018
                strSql = "SELECT TM_TAX_INDEX, TM_TAX_CODE, TM_TAX_DESC, TM_BRANCH_CODE, TM_COST_CENTER, TGM_GL_CODE, IFNULL(CBG_B_GL_DESC, 'N/A') AS GL_CODE_DESC, " &
                        "CASE WHEN TM_TAX_TYPE = 'P' THEN 'Purchase' ELSE 'Supply' END AS TAXTYPE, TM_TAX_TYPE, " &
                        "IF(TAX_PERC = '', CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS TAXRATE, TM_TAX_RATE " &
                        "FROM TAX_MSTR " &
                        "INNER JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = TM_TAX_RATE " &
                        "INNER JOIN TAX ON TAX_CODE = TM_TAX_RATE AND TAX_COUNTRY_CODE = 'MY' " &
                        "INNER JOIN TAX_GL_MSTR ON TGM_TAX_INDEX = TM_TAX_INDEX " &
                        "LEFT JOIN COMPANY_B_GL_CODE ON CBG_B_GL_CODE = TGM_GL_CODE AND CBG_B_COY_ID = '" & strDefIPPCompID & "' " &
                        "WHERE TM_DELETED <> 'Y' AND TM_CATEGORY = '" & strCat & "' "
            Else
                strSql = "SELECT TM_TAX_INDEX, TM_TAX_CODE, TM_TAX_DESC, CM_CT.CODE_DESC AS COUNTRY, TM_COUNTRY_CODE, " &
                        "CASE WHEN TM_TAX_TYPE = 'P' THEN 'Purchase' ELSE 'Supply' END AS TAXTYPE, TM_TAX_TYPE, " &
                        "IF(TAX_PERC = '', CM_GST.CODE_DESC, CONCAT(CM_GST.CODE_DESC, ' (', TAX_PERC, '%)')) AS TAXRATE, TM_TAX_RATE " &
                        "FROM TAX_MSTR " &
                        "INNER JOIN CODE_MSTR CM_CT ON CM_CT.CODE_CATEGORY = 'CT' AND CM_CT.CODE_ABBR = TM_COUNTRY_CODE " &
                        "INNER JOIN CODE_MSTR CM_GST ON CM_GST.CODE_CATEGORY = 'SST' AND CM_GST.CODE_ABBR = TM_TAX_RATE " &
                        "INNER JOIN TAX ON TAX_CODE = TM_TAX_RATE AND TAX_COUNTRY_CODE = 'MY' " &
                        "WHERE TM_DELETED <> 'Y' AND TM_CATEGORY = '" & strCat & "' "
            End If

            If strCoyId <> "" Then
                strSql &= " AND TM_COY_ID = '" & Common.Parse(strCoyId) & "'"
            End If

            If strCode <> "" Then
                strSql &= " AND TM_TAX_CODE LIKE '%" & Common.Parse(strCode) & "%'"
            End If

            strSql &= " ORDER BY TM_TAX_CODE "

            ds = objDb.FillDs(strSql)
            getTaxCodeInfo = ds
        End Function

        Public Function AddTaxCodeInfo(ByVal strTaxCode As String, ByVal strDesc As String, ByVal strCountryCode As String, ByVal strType As String, ByVal strRate As String, ByVal strMode As String,
                                    Optional ByVal strIndex As String = "", Optional ByVal strCat As String = "eProcure", Optional ByVal strGL As String = "", Optional ByVal strBC As String = "", Optional ByVal strCC As String = "")
            Dim strCoyId, strUserID, strSQL As String
            Dim ds As New DataSet
            Dim strAry(0) As String
            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            If strMode = "add" Then
                If strCat = "eProcure" Then
                    strSQL = "SELECT '*' FROM TAX_MSTR " &
                            "WHERE TM_TAX_CODE = '" & Common.Parse(strTaxCode) & "' AND TM_CATEGORY = '" & strCat & "' " &
                            "AND TM_DELETED <> 'Y'"
                ElseIf strCat = "IPP" Then
                    strSQL = "SELECT '*' FROM TAX_MSTR " &
                            "WHERE TM_TAX_CODE = '" & Common.Parse(strTaxCode) & "' AND TM_CATEGORY = '" & strCat & "' " &
                            "AND TM_DELETED <> 'Y' AND TM_COY_ID = '" & strCoyId & "'"
                End If

                If objDb.Exist(strSQL) > 0 Then
                    Return -99
                End If

                If strCat = "eProcure" Then
                    strSQL = "INSERT INTO TAX_MSTR (TM_TAX_CODE, TM_TAX_DESC, TM_TAX_TYPE, TM_TAX_RATE, TM_COUNTRY_CODE, TM_CATEGORY, TM_DELETED, TM_ENT_BY, TM_ENT_DATETIME) " &
                            "VALUES ('" & Common.Parse(strTaxCode) & "', '" & Common.Parse(strDesc) & "', '" & strType & "', '" & strRate & "' , '" & strCountryCode & "', 'eProcure', 'N', 'hubadmin', " & Common.ConvertDate(Now) & ")"
                ElseIf strCat = "IPP" Then
                    strSQL = "INSERT INTO TAX_MSTR (TM_TAX_CODE, TM_TAX_DESC, TM_TAX_TYPE, TM_TAX_RATE, TM_COUNTRY_CODE, TM_CATEGORY, TM_DELETED, TM_ENT_BY, TM_ENT_DATETIME, TM_BRANCH_CODE, TM_COST_CENTER, TM_COY_ID) " &
                            "VALUES ('" & Common.Parse(strTaxCode) & "', '" & Common.Parse(strDesc) & "', '" & strType & "', '" & strRate & "' , '" & strCountryCode & "', 'IPP', 'N', '" & strUserID & "', " & Common.ConvertDate(Now) & ", '" & Common.Parse(strBC) & "', '" & Common.Parse(strCC) & "', '" & strCoyId & "')"
                End If

                Common.Insert2Ary(strAry, strSQL)

                If strCat = "IPP" Then
                    strSQL = "INSERT INTO TAX_GL_MSTR (TGM_TAX_INDEX, TGM_GL_CODE, TGM_ENT_BY, TGM_ENT_DATETIME) " &
                            "VALUES (" & objDb.GetLatestInsertedID("TAX_MSTR") & ", '" & Common.Parse(strGL) & "', '" & strUserID & "', " & Common.ConvertDate(Now) & ")"
                    Common.Insert2Ary(strAry, strSQL)
                End If
            Else
                If strCat = "eProcure" Then
                    strSQL = "UPDATE TAX_MSTR SET " &
                            "TM_TAX_DESC = '" & Common.Parse(strDesc) & "', " &
                            "TM_TAX_TYPE = '" & strType & "', " &
                            "TM_TAX_RATE = '" & strRate & "', " &
                            "TM_COUNTRY_CODE = '" & strCountryCode & "', " &
                            "TM_MOD_BY = 'hubadmin', " &
                            "TM_MOD_DATETIME = " & Common.ConvertDate(Now) & " " &
                            "WHERE TM_TAX_INDEX = '" & strIndex & "'"
                ElseIf strCat = "IPP" Then
                    strSQL = "UPDATE TAX_MSTR SET " &
                            "TM_TAX_DESC = '" & Common.Parse(strDesc) & "', " &
                            "TM_TAX_TYPE = '" & strType & "', " &
                            "TM_TAX_RATE = '" & strRate & "', " &
                            "TM_BRANCH_CODE = '" & Common.Parse(strBC) & "', " &
                            "TM_COST_CENTER = '" & Common.Parse(strCC) & "', " &
                            "TM_MOD_BY = '" & strUserID & "', " &
                            "TM_MOD_DATETIME = " & Common.ConvertDate(Now) & " " &
                            "WHERE TM_TAX_INDEX = '" & strIndex & "'"
                End If

                Common.Insert2Ary(strAry, strSQL)

                If strCat = "IPP" Then
                    strSQL = "UPDATE TAX_GL_MSTR SET " &
                            "TGM_GL_CODE = '" & Common.Parse(strGL) & "', " &
                            "TGM_MOD_BY = '" & strUserID & "', " &
                            "TGM_MOD_DATETIME = " & Common.ConvertDate(Now) & " " &
                            "WHERE TGM_TAX_INDEX = '" & strIndex & "'"
                    Common.Insert2Ary(strAry, strSQL)
                End If
            End If

            If objDb.BatchExecute(strAry) Then
                AddTaxCodeInfo = WheelMsgNum.Save
            Else
                AddTaxCodeInfo = WheelMsgNum.NotSave
            End If

        End Function

        Public Function delTaxCodeInfo(ByVal dtTax As DataTable, Optional ByVal strCat As String = "eProcure") As Integer
            Dim strAry(0) As String
            Dim strSql As String
            Dim drTax As DataRow

            For Each drTax In dtTax.Rows
                'strSql = "SELECT '*' FROM TAX_DETAIL WHERE TD_TAX_INDEX =" & Common.Parse(drTax("TaxIndex"))
                'If objDb.Exist(strSql) <> 0 Then
                '    Return -99
                'End If
                If strCat = "IPP" Then
                    strSql = "UPDATE TAX_MSTR SET " &
                        "TM_DELETED = 'Y', " &
                        "TM_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', " &
                        "TM_MOD_DATETIME = " & Common.ConvertDate(Now) & " " &
                        "WHERE TM_TAX_INDEX = '" & drTax("TaxIndex") & "'"
                    Common.Insert2Ary(strAry, strSql)
                Else
                    strSql = "UPDATE TAX_MSTR SET " &
                        "TM_DELETED = 'Y', " &
                        "TM_MOD_BY = 'hubadmin', " &
                        "TM_MOD_DATETIME = " & Common.ConvertDate(Now) & " " &
                        "WHERE TM_TAX_INDEX = '" & drTax("TaxIndex") & "'"
                    Common.Insert2Ary(strAry, strSql)
                End If
            Next

            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function
        'Zulham 15082018 - PAMB
        Public Sub delDept(ByVal strDeptCode As String) 'As Integer
            Dim strAry(0) As String
            Dim strSQL, strUserID As String
            Dim drDept As DataRow

            strSQL = "update company_dept_mstr set cdm_deleted='Y' where cdm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' and cdm_dept_code='" & Common.Parse(strDeptCode) & "' and cdm_approval_grp_index IS NULL "
            Common.Insert2Ary(strAry, strSQL)
            objDb.BatchExecute(strAry)

        End Sub
    End Class
End Namespace

