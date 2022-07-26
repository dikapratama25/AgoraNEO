Imports System
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports AgoraNEO
Imports SSO.Component

Namespace AgoraNEO
    Public Class Admin_Ext
        Dim objDb As New EAD.DBCom
        Dim lsSql As String

        Public Function PopulateMfgName(ByVal strProdCode As String, ByVal CompanyId As String) As DataSet
            Dim strSql As String
            Dim dsMfgName As DataSet

            strSql = "SELECT PM_MANUFACTURER, PM_MANUFACTURER2, PM_MANUFACTURER3  FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & strProdCode & "' AND PM_S_COY_ID = '" & CompanyId & "'"
            dsMfgName = objDb.FillDs(strSql)

            PopulateMfgName = dsMfgName
        End Function

        Public Function GetVendorCodeInfo(ByVal strVendorID As String, ByVal CompanyId As String) As DataSet
            Dim strSql, strCoyId As String
            Dim ds As DataSet

            strCoyId = CompanyId

            strSql = "SELECT CVS_SUPP_CODE, CVS_DELIVERY_TERM, CVS_CURR " &
                    "FROM COMPANY_VENDOR_SUPPCODE " &
                    "WHERE CVS_B_COY_ID = '" & strCoyId & "' AND CVS_S_COY_ID = '" & strVendorID & "'"
            ds = objDb.FillDs(strSql)
            GetVendorCodeInfo = ds

        End Function

        Public Function delvendor(ByVal strVendorID As String, ByVal aryList As ArrayList, ByVal CompanyId As String) As Integer
            Dim strSql_delvendor As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            strSql_delvendor = "delete FROM COMPANY_VENDOR where CV_B_COY_ID ='" & CompanyId & "' and CV_S_COY_ID= '" & strVendorID & "'"
            AgoraNEO.Common.Insert2Ary(strAryQuery, strSql_delvendor)

            If Not aryList Is Nothing Then
                For i = 0 To aryList.Count - 1
                    If strVendorID = aryList(i)(3) Then
                        strSql_delvendor = "DELETE FROM COMPANY_VENDOR_SUPPCODE " &
                                        "WHERE CVS_B_COY_ID = '" & CompanyId & "' " &
                                        "AND CVS_S_COY_ID = '" & Common.Parse(strVendorID) & "'"
                        Common.Insert2Ary(strAryQuery, strSql_delvendor)
                        Exit For
                    End If
                Next
            End If

            If objDb.BatchExecute(strAryQuery) Then
                delvendor = WheelMsgNum.Delete
            Else
                delvendor = WheelMsgNum.NotDelete
            End If
        End Function

        Public Function delVendorCode(ByVal strVendorID As String, ByVal CompanyId As String) As String
            Dim strDel As String

            strDel = "DELETE FROM COMPANY_VENDOR_SUPPCODE " &
                    "WHERE CVS_B_COY_ID = '" & CompanyId & "' " &
                    "AND CVS_S_COY_ID = '" & Common.Parse(strVendorID) & "'"

            delVendorCode = strDel
        End Function

        Public Function upVendorCode(ByVal strVendorID As String, ByVal strVenCode As String, ByVal strDelTerm As String, ByVal strCurr As String, ByVal CompanyId As String) As String
            Dim strUpdate As String

            strUpdate = "INSERT INTO COMPANY_VENDOR_SUPPCODE " &
                    "(CVS_B_COY_ID, CVS_S_COY_ID, CVS_SUPP_CODE, CVS_DELIVERY_TERM, CVS_CURR) " &
                    "VALUES('" & CompanyId & "', '" & Common.Parse(strVendorID) & "', " &
                    "'" & Common.Parse(strVenCode) & "', '" & Common.Parse(strDelTerm) & "', '" & Common.Parse(strCurr) & "')"

            upVendorCode = strUpdate
        End Function

        Public Function getVenDefaultPayTerm(ByVal strCode As String, ByVal intRow As Integer, ByVal strVendorID As String, ByVal CompanyId As String) As String
            Dim strSql As String
            Dim ds As New DataSet
            Dim strPrevVendor As String

            If intRow = 0 Then
                strSql = "SELECT IFNULL(PV_S_COY_ID,'') FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = 'P' AND PV_PRODUCT_INDEX = (SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.Parse(strCode) & "' LIMIT 1)"
            Else
                strSql = "SELECT IFNULL(PV_S_COY_ID,'') FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '" & CStr(intRow) & "' AND PV_PRODUCT_INDEX = (SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.Parse(strCode) & "' LIMIT 1)"
            End If
            ds = objDb.FillDs(strSql)
            If ds.Tables(0).Rows.Count > 0 Then
                strPrevVendor = ds.Tables(0).Rows(0)(0)
            Else
                strPrevVendor = ""
            End If

            If strVendorID <> strPrevVendor Then
                strSql = "SELECT IFNULL(CV_PAYMENT_TERM,'') FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & CompanyId & "' AND CV_S_COY_ID = '" & Common.Parse(strVendorID) & "'"
                getVenDefaultPayTerm = objDb.GetVal(strSql)
            Else
                getVenDefaultPayTerm = ""
            End If

        End Function
    End Class
End Namespace

