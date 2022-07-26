Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports AgoraLegacy
Imports SSO.Component
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Diagnostics
Imports CryptoClass
Imports System.Security.Cryptography

Namespace AgoraNEO

    Public Class GST
        Dim objDb As New EAD.DBCom
        Dim v_com_id As String = HttpContext.Current.Session("CompanyId")
        Dim com_id As String = HttpContext.Current.Session("CompanyId")
        Dim userid As String = HttpContext.Current.Session("UserId")

        'Chee Hong - 13/08/2014 - Function to check the GST whether is valid or invalid
        Public Function chkGST(Optional ByVal strCoyId As String = "", Optional ByVal strDODate As String = "", Optional ByVal blnCheckCOD As Boolean = True) As String
            Dim strSQL As String
            Dim strGstCOD As String
            strGstCOD = ConfigurationManager.AppSettings.Get("GstCutOffDate")

            If strCoyId = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            End If

            If Date.Now() >= CDate(strGstCOD) Or Not blnCheckCOD Then
                strSQL = "SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyId & "'"
                chkGST = objDb.GetVal(strSQL)
            Else
                chkGST = ""
            End If

            If chkGST <> "" And strDODate <> "" Then
                If CDate(strDODate) < CDate(strGstCOD) Then
                    chkGST = ""
                End If
            End If

        End Function

        'Chee Hong - 13/08/2014 - Function to check the GST cut off date
        Public Function chkGSTCOD(Optional ByVal strDocDate As String = "") As Boolean
            Dim strGstCOD As String
            strGstCOD = ConfigurationManager.AppSettings.Get("GstCutOffDate")

            If strDocDate <> "" Then
                If CDate(strDocDate) >= CDate(strGstCOD) Then
                    chkGSTCOD = True
                Else
                    chkGSTCOD = False
                End If
            Else
                If Date.Now() >= CDate(strGstCOD) Then
                    chkGSTCOD = True
                Else
                    chkGSTCOD = False
                End If
            End If
        End Function

        'Chee Hong - 19/08/2014 - Function to check the GST based on GST Rate
        Public Function chkGSTByRate(ByVal strDocType As String, ByVal strDocID As String, Optional ByVal strCompId As String = "") As Boolean
            Dim strSql As String

            If strCompId = "" Then
                strCompId = HttpContext.Current.Session("CompanyId")
            End If

            If strDocType = "QUO" Then
                strSql = "SELECT '*' FROM RFQ_REPLIES_DETAIL " &
                        "WHERE (RRD_GST_RATE <> '' AND RRD_GST_RATE IS NOT NULL) " &
                        "AND RRD_V_COY_ID = '" & strCompId & "' AND RRD_RFQ_ID = '" & strDocID & "'"
            End If

            If objDb.Exist(strSql) > 0 Then
                chkGSTByRate = True
            Else
                chkGSTByRate = False
            End If

        End Function

        Public Sub getGSTInfobyRate(ByVal strGSTRate As String, ByRef strGSTPerc As String, ByRef strGSTID As String)
            Dim strSql, strCountry As String
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet

            strSql = "SELECT CM_COUNTRY FROM COMPANY_MSTR WHERE CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
            strCountry = objDB.GetVal(strSql)

            strSql = "SELECT TAX_PERC, IFNULL(TAX_AUTO_NO, 0) AS TAX_ID FROM TAX " &
                     "INNER JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND TAX_CODE = CODE_ABBR " &
                     "WHERE TAX_COUNTRY_CODE = '" & strCountry & "' AND CODE_ABBR = '" & strGSTRate & "'"

            ds = objDB.FillDs(strSql)
            If ds.Tables(0).Rows.Count > 0 Then
                If strGSTRate = "N/A" Then
                    strGSTID = "0"
                    strGSTPerc = "0"
                Else
                    strGSTID = ds.Tables(0).Rows(0).Item("TAX_ID")
                    strGSTPerc = ds.Tables(0).Rows(0).Item("TAX_PERC")
                End If
            End If
            ds = Nothing
            objDB = Nothing
        End Sub

        Public Function getGstDesc(ByVal strGstRate As String) As String
            Dim strSql As String
            Dim objDB As New EAD.DBCom

            strSql = "SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY = 'SST' AND CODE_ABBR = '" & strGstRate & "'"
            getGstDesc = objDB.GetVal(strSql)
            objDB = Nothing

        End Function

        Public Function chkValidTaxCode(ByVal strGstRate As String, ByVal strTaxCode As String, ByVal strType As String, Optional ByVal strCategory As String = "eProcure") As Boolean
            Dim strSql As String

            strSql = "SELECT '*' FROM TAX_MSTR " &
                    "INNER JOIN COMPANY_MSTR ON CM_COUNTRY = TM_COUNTRY_CODE " &
                    "WHERE TM_TAX_TYPE = '" & strType & "' AND TM_TAX_RATE = '" & strGstRate & "' " &
                    "AND TM_CATEGORY = '" & strCategory & "' AND TM_TAX_CODE = '" & Common.Parse(strTaxCode) & "' " &
                    "AND TM_DELETED = 'N' " &
                    "AND CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            If objDb.Exist(strSql) > 0 Then
                chkValidTaxCode = True
            Else
                chkValidTaxCode = False
            End If

        End Function

        Public Function getGSTRateDescriptionbyRate(ByVal strGSTRate As String) As String
            Dim strSql, strResult As String
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet

            strSql = "SELECT IF(TAX_PERC = '', CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST, CODE_ABBR " &
                    "FROM CODE_MSTR " &
                    "INNER JOIN TAX ON TAX_CODE = CODE_ABBR " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND TAX_COUNTRY_CODE = CM_COUNTRY " &
                    "AND CODE_ABBR = '" & strGSTRate & "'"

            ds = objDB.FillDs(strSql)
            If ds.Tables(0).Rows.Count > 0 Then
                strResult = ds.Tables(0).Rows(0).Item("GST")
            End If
            ds = Nothing
            objDB = Nothing
            Return strResult
        End Function

        Public Function getDefaultVendorItemGSTRate(ByVal strVendorItemCode As String, ByVal strVendorCoy As String) As String
            Dim strSql, strResult As String
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet

            strSql = "SELECT PRODUCT_MSTR.PM_GST_CODE FROM PRODUCT_MSTR WHERE PRODUCT_MSTR.PM_VENDOR_ITEM_CODE = '" & strVendorItemCode & "'" &
                    "AND PRODUCT_MSTR.PM_S_COY_ID = '" & strVendorCoy & "' AND PRODUCT_MSTR.PM_PRODUCT_FOR = 'V'"
            ds = objDB.FillDs(strSql)
            If ds.Tables(0).Rows.Count > 0 Then
                strResult = ds.Tables(0).Rows(0).Item("PM_GST_CODE")
            End If
            ds = Nothing
            objDB = Nothing
            Return strResult
        End Function

        Public Sub FillTaxCode_forIPP(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strRate As String = "", Optional ByVal strType As String = "", Optional ByVal strCategory As String = "eProcure", Optional ByVal blnSelect As Boolean = True)
            Dim strDefaultValue As String
            Dim strSql, strCountry As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            'IPP Gst Stage 2A - CH - 11 Feb 2015
            'strSql = "SELECT IF(TAX_PERC = '', TM_TAX_CODE, CONCAT(TM_TAX_CODE, ' (', TAX_PERC, '%)')) AS GST, TM_TAX_CODE FROM tax_mstr, tax WHERE " & _
            '         "tm_coy_id = 'hlb' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code "
            strSql = "SELECT IF(TAX_PERC = '', TM_TAX_CODE, CONCAT(TM_TAX_CODE, ' (', TAX_PERC, '%)')) AS GST, TM_TAX_CODE FROM tax_mstr, tax WHERE " &
                                 "tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code "

            If strRate <> "" Then
                strSql &= " AND TM_TAX_RATE = '" & strRate & "' "
            End If

            If strType <> "" Then
                strSql &= " AND TM_TAX_TYPE = '" & strType & "' "
            End If

            strSql &= " ORDER BY TM_TAX_CODE "

            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "GST", "TM_TAX_CODE", drw)
                If blnSelect = True Then
                    lstItem.Value = ""
                    lstItem.Text = "---Select---"
                    strDefaultValue = lstItem.Text
                    pDropDownList.Items.Insert(0, lstItem)
                End If
            Else
                lstItem.Value = "N/A"
                lstItem.Text = "N/A"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                Common.SelDdl(strDefaultValue, pDropDownList, True, True)
            End If
            objDB = Nothing
        End Sub

        Public Function GetTaxCode_forIPP(Optional ByVal strRate As String = "", Optional ByVal strType As String = "", Optional ByVal strCategory As String = "eProcure") As DataSet
            Dim strDefaultValue As String
            Dim strSql, strCountry As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom

            'IPP Gst Stage 2A - CH - 11 Feb 2015
            'strSql = "SELECT IF(TAX_PERC = '', TM_TAX_CODE, CONCAT(TM_TAX_CODE, ' (', TAX_PERC, '%)')) AS GST, TM_TAX_CODE FROM tax_mstr, tax WHERE " & _
            '         "tm_coy_id = 'hlb' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code "
            strSql = "SELECT IF(TAX_PERC = '', TM_TAX_CODE, CONCAT(TM_TAX_CODE, ' (', TAX_PERC, '%)')) AS GST, TM_TAX_CODE FROM tax_mstr, tax WHERE " &
                     "tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code "

            If strRate <> "" Then
                strSql &= " AND TM_TAX_RATE = '" & strRate & "' "
            End If

            If strType <> "" Then
                strSql &= " AND TM_TAX_TYPE = '" & strType & "' "
            End If

            'Jules 2018.09.19 - Filter out for IPP to avoid dupes.
            If strCategory = "IPP" Then
                strSql &= " And TM_CATEGORY='" & strCategory & "' "
            End If
            'End modification.

            strSql &= " ORDER BY TM_TAX_CODE "

            ds = objDB.FillDs(strSql)
            Return ds

        End Function

        'Jules 2018.08.28 - For FFPO.
        Public Function GetTaxCode_forP2P(Optional ByVal strRate As String = "", Optional ByVal strType As String = "") As DataSet
            Dim strSql As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom

            strSql = "SELECT IF(TAX_PERC = '', TM_TAX_CODE, CONCAT(TM_TAX_CODE, ' (', TAX_PERC, '%)')) AS GST, TM_TAX_CODE FROM TAX_MSTR " &
                    "INNER JOIN TAX ON TM_TAX_RATE = TAX_CODE " &
                    "INNER JOIN COMPANY_MSTR On CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "WHERE TM_DELETED <> 'Y' AND TM_CATEGORY = 'eProcure' " &
                    "AND TM_COUNTRY_CODE = CM_COUNTRY "

            If strRate <> "" Then
                strSql &= " AND TM_TAX_RATE = '" & strRate & "' "
            End If

            If strType <> "" Then
                strSql &= " AND TM_TAX_TYPE = '" & strType & "' "
            End If

            strSql &= " ORDER BY TM_TAX_CODE "

            ds = objDB.FillDs(strSql)
            Return ds

        End Function
        'End modification.

        Public Function chkGST_ForIPP(ByVal strCoyIdx As String) As String

            Dim strSQL As String
            strSQL = "SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_index = '" & strCoyIdx & "'"
            chkGST_ForIPP = objDb.GetVal(strSQL)

        End Function

        Public Function chkExistTaxCode(ByVal strTaxCode As String, ByVal strType As String, ByVal strCat As String) As Boolean
            Dim strSql As String

            strSql = "SELECT * FROM TAX_MSTR " &
                    "WHERE TM_DELETED = 'N' AND TM_TAX_CODE = '" & Common.Parse(strTaxCode) & "' " &
                    "AND TM_TAX_TYPE = '" & strType & "' AND TM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND TM_CATEGORY = '" & strCat & "' "
            If objDb.Exist(strSql) > 0 Then
                chkExistTaxCode = True
            Else
                chkExistTaxCode = False
            End If
        End Function

        Public Sub getGSTInfobyRate_ForIPP(ByVal strGSTCode As String, ByRef strGSTPerc As String, ByRef strGSTRate As String)
            Dim strSql, strCountry As String
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet

            'IPP Gst Stage 2A - CH - 11 Feb 2015
            'strSql = "SELECT TM_TAX_RATE, IF(TRIM(TAX_PERC) = '',0,TAX_PERC) TAX_PERC FROM tax_mstr, tax WHERE " & _
            '            "tm_coy_id = 'hlb' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code " & _
            '            "AND tm_tax_code = '" & strGSTCode & "'"
            strSql = "SELECT TM_TAX_RATE, IF(TRIM(TAX_PERC) = '',0,TAX_PERC) TAX_PERC FROM tax_mstr, tax WHERE " &
                    "tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code " &
                    "AND tm_tax_code = '" & strGSTCode & "'"

            ds = objDB.FillDs(strSql)
            If ds.Tables(0).Rows.Count > 0 Then
                If strGSTCode = "N/A" Then
                    strGSTRate = "N/A"
                    strGSTPerc = ""
                Else
                    strGSTRate = ds.Tables(0).Rows(0).Item("TM_TAX_RATE")
                    strGSTPerc = ds.Tables(0).Rows(0).Item("TAX_PERC")
                End If
            End If
            ds = Nothing
            objDB = Nothing
        End Sub

        Public Function GetTaxInfoByTaxCode_forIPP(ByVal strTaxCode As String) As DataSet
            'Zulham 18052015 IPP GST Stage 1
            Dim strDefaultValue As String
            Dim strSql, strCountry As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom

            strSql = "SELECT IF(TAX_PERC = '', CONCAT(TM_TAX_CODE, ' (0%)'), CONCAT(TM_TAX_CODE, ' (', TAX_PERC, '%)')) AS GST, TM_TAX_CODE FROM tax_mstr, tax WHERE " &
                     "tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code " &
                     "and tm_tax_code = '" & strTaxCode.Trim & "'"

            strSql &= " ORDER BY TM_TAX_CODE "

            ds = objDB.FillDs(strSql)
            Return ds

        End Function

        'Zulham 14/01/2016 - IPP Stage 4 Phase 2
        'To get the percentage of certain tax code
        Public Function getTaxPercentage(ByVal taxCode As String) As String
            Dim strPercentage As String = 1
            Dim dsResult As DataSet
            Dim objDB As New EAD.DBCom
            Dim strSql As String
            strSql = "SELECT IFNULL(tax_perc,0) 'percentage' FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND " &
            "tm_tax_code = '" & taxCode & "' AND tax_code = tm_tax_rate LIMIT 1"
            dsResult = objDB.FillDs(strSql)
            If dsResult.Tables(0).Rows.Count > 0 Then
                strPercentage = dsResult.Tables(0).Rows(0).Item("percentage")
            End If

            Return strPercentage

        End Function

        'Jules 2018.10.08 - Check whether document is Service Tax or Sales Tax document.
        Public Function chkDocumentType(ByVal strDocNo As String, ByVal strDocType As String, Optional ByVal strParam As String = "", Optional ByVal strVendorId As String = "", Optional ByVal strBuyerId As String = "", Optional ByVal strCoyType As String = "B", Optional ByRef strTaxType As String = "") As Boolean
            Dim strSSTStartDate, strSQL, strDocDate, strBCoyId, strVCoyId As String
            Dim dsResult As DataSet
            Dim blnSST As Boolean = False
            'strSSTStartDate = System.Configuration.ConfigurationSettings.AppSettings.Get("SSTStartDate")
            strSSTStartDate = "01/09/2018"

            If strVendorId <> "" Then
                strVCoyId = strVendorId
            Else
                strVCoyId = HttpContext.Current.Session("CompanyId")
            End If

            If strBuyerId <> "" Then
                strBCoyId = strBuyerId
            Else
                strBCoyId = HttpContext.Current.Session("CompanyId")
            End If

            'Check if company is SST-registered.
            Dim strSSTRegNo As String = ""
            If strCoyType = "B" Then
                strSSTRegNo = objDb.GetVal("SELECT CM_TAX_REG_NO FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strBCoyId & "'")
            Else
                strSSTRegNo = objDb.GetVal("SELECT CM_TAX_REG_NO FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strVCoyId & "'")
            End If


            If strSSTRegNo <> "" Then
                blnSST = True
                If strDocType = "INV" Then
                    strSQL = "SELECT '*' FROM INVOICE_MSTR " &
                            "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO=ID_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID " &
                            "WHERE IM_INVOICE_NO ='" & strDocNo & "' AND IM_B_COY_ID='" & strBCoyId & "' AND IM_S_COY_ID='" & strVCoyId & "' " &
                            "AND (ID_GST_RATE = 'ST5' OR ID_GST_RATE = 'ST10') "

                ElseIf strDocType = "CN" Then
                    strSQL = "SELECT '*' FROM CREDIT_NOTE_MSTR " &
                            "INNER JOIN CREDIT_NOTE_DETAILS ON CNM_CN_NO=CND_CN_NO AND CND_CN_S_COY_ID=CNM_CN_S_COY_ID " &
                            "WHERE CNM_CN_B_COY_ID = '" & strBCoyId & "' AND CNM_CN_NO='" & strDocNo & "' AND CNM_CN_S_COY_ID='" & strVCoyId & "' " &
                            "AND (CND_GST_RATE = 'ST5' OR CND_GST_RATE = 'ST10') "

                ElseIf strDocType = "DN" Then
                    strSQL = "SELECT '*' FROM DEBIT_NOTE_MSTR " &
                            "INNER JOIN DEBIT_NOTE_DETAILS ON DNM_DN_NO=DND_DN_NO AND DND_DN_S_COY_ID=DNM_DN_S_COY_ID " &
                            "WHERE DNM_DN_B_COY_ID = '" & strBCoyId & "' AND DNM_DN_NO='" & strDocNo & "' AND DNM_DN_S_COY_ID='" & strVCoyId & "' " &
                            "AND (DND_GST_RATE = 'ST5' OR DND_GST_RATE = 'ST10') "

                ElseIf strDocType = "PO" Then
                    strSQL = "SELECT '*' FROM PO_MSTR INNER JOIN PO_DETAILS ON POM_PO_NO=POD_PO_NO AND POM_B_COY_ID=POD_COY_ID " &
                            "WHERE POM_PO_NO='" & strDocNo & "' AND POM_B_COY_ID='" & strBCoyId & "' " &
                            "AND (POD_GST_RATE = 'ST5' OR POD_GST_RATE = 'ST10') "

                ElseIf strDocType = "PR" Then
                    strSQL = "SELECT * FROM PR_MSTR " &
                            "INNER JOIN PR_DETAILS ON PRM_PR_NO=PRD_PR_NO AND PRM_COY_ID=PRD_COY_ID " &
                            "WHERE PRM_PR_INDEX='" & strDocNo & "' " &
                            "AND (PRD_GST_RATE = 'ST5' OR PRD_GST_RATE = 'ST10') "

                ElseIf strDocType = "QTN" Then
                    strSQL = "SELECT * FROM RFQ_MSTR " &
                            "INNER JOIN RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID " &
                            "INNER JOIN RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID " &
                            "AND RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id " &
                            "WHERE RFQ_REPLIES_MSTR.RRM_V_Company_ID = '" & strVendorId & "' AND RFQ_MSTR.RM_Coy_ID = '" & strBuyerId & "' " &
                            "AND RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = '" & strDocNo & "' AND RFQ_MSTR.RM_RFQ_No = '" & strParam & "' " &
                            "AND (RRD_GST_RATE = 'ST5' OR RRD_GST_RATE = 'ST10') "
                End If

                If objDb.Exist(strSQL) > 0 Then
                    strTaxType = "Sales"
                Else
                    strTaxType = "Service"
                End If

            Else
                blnSST = False
            End If
            Return blnSST
        End Function
    End Class

End Namespace
