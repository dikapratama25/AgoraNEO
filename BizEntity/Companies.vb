Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Web
Imports AgoraNEO

Namespace AgoraNEO
    Public Class Company
        Public CoyId As String
        Public CoyName As String
        Public ParentCoy As String
        Public AccountNo As String
        Public BankCode As String
        Public BranchCode As String
        Public BankName As String
        Public Currency As String
        Public Address1 As String
        Public Address2 As String
        Public Address3 As String
        Public City As String
        Public State As String
        Public PostCode As String
        Public Country As String
        Public Phone As String
        Public Fax As String
        Public Email As String
        Public CoyLogo As String
        Public BusinessRegNo As String
        'Public GSTRegNo As String
        Public TaxCalBy As String
        Public PaymentMethod As String
        Public PaymentTerm As String
        Public PwdDuration As String
        Public Status As String
        Public CoyType As String
        Public BCMSetting As String
        Public FinDeptMode As String
        Public PrivLabeling As String
        Public Skins As String
        Public TrainDemo As String
        Public LicenseUsers As String
        Public SubStart As String
        Public SubEnd As String
        Public TaxRegNo As String
        Public GSTDateLastStatus As String
        Public TC As String
        Public Actual_TC As String
        Public Package As String
        Public BCMStart As String
        Public BCMEnd As String
        Public SKU As String
        Public TransNo As String
        Public ContactPerson As String
        Public ReportUsers As String
        Public InvAppr As String
        Public MultiInvAppr As String
        Public BACanPO As String
        Public CoyLongName As String
        Public WebSites As String
        Public RegYear As String
        Public RegCurrency As String
        Public PaidUpCapital As String
        Public Ownership As String
        Public OwnershipOthers As String
        Public Business As String
        Public Commodity As String
        Public OrgCode As String
        Public ContrPRSetting As String
        'Jules
        Public BOtherCoy As String
        Public BankAddrLine1 As String
        Public BankAddrLine2 As String
        Public BankAddrLine3 As String
        Public BankCity As String
        Public BankState As String
        Public BankPostcode As String
        Public BankCountry As String
        Public ConIBSGLCode As String
        Public NonConIBSGLCode As String
        'Public RelatedCompany As String
        Public WaiveCharges As String
        Public CreditTerms As String
        Public InactiveReason As String
        Public CompanyCategory As String
        Public ResidentType As String
        Public ResidentCountry As String
        Public GstInputTaxCode As String
        Public GstOutputTaxCode As String
        Public JobGrade As String
        Public CostCentre As String
        Public StaffCessationEffectiveDate As String
		'Modified for IPP Gst Stage 2A
        Public GstRegDate As String 
        Public NostroIncome As String
        Public BillGLCode As String
        Public SysValDate As String ' Jules added for IPP Stage 4 Phase 2 2015.08.14
		'-----------------------------
    End Class

    Public Class Companies
        Dim objDb As New EAD.DBCom
        Dim strCompMassage As String
        'Dim ctx As Web.HttpContext = Web.HttpContext.Current

        Public Property Message() As String
            Get
                Message = strCompMassage
            End Get
            Set(ByVal Value As String)
                strCompMassage = Value
            End Set
        End Property

        'Public Function getGSTRegNo(Optional ByVal strCoyId As String = "") As String
        '    Dim strSQL As String
        '    Dim strGstCOD As String
        '    strGstCOD = ConfigurationManager.AppSettings.Get("GstCutOffDate")

        '    If strCoyId = "" Then
        '        strCoyId = HttpContext.Current.Session("CompanyID")
        '    End If

        '    If Date.Now() >= CDate(strGstCOD) Then
        '        strSQL = "SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyId & "'"
        '        getGSTRegNo = objDb.GetVal(strSQL)
        '    Else
        '        getGSTRegNo = ""
        '    End If

        'End Function

        'Public Function AddCompany(ByVal pComp As Company, Optional ByVal strApp() As String = Nothing) As Boolean
        '    AddCompany = False

        '    Dim strSql, strSqlExist, strSqlDel As String

        '    strSqlExist = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
        '          Common.Parse(pComp.CoyId) & "' AND CM_DELETED <>'Y'"

        '    strSqlDel = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
        '           Common.Parse(pComp.CoyId) & "' AND CM_DELETED='Y'"
        '    Try

        '        If objDb.Exist(strSqlExist) Then
        '            strCompMassage = Common.RecordDuplicate
        '            Return False
        '        ElseIf objDb.Exist(strSqlDel) Then
        '            strCompMassage = Common.RecordUsed
        '            Return False
        '        Else
        '            ' ai chu add on 05/12/2005
        '            ' SR U30012 - to include additional fields to allow hub admin enter the 
        '            ' no of SKU and no of transacrion at the company detail screen
        '            strSql = "INSERT INTO COMPANY_MSTR(CM_COY_ID,CM_COY_NAME,CM_STATUS," &
        '                    "CM_COY_TYPE,CM_LICENCE_PACKAGE, " &
        '                    "CM_LICENSE_USERS,CM_SUB_START_DT,CM_SUB_END_DT," &
        '                    "CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3," &
        '                    "CM_CITY,CM_STATE,CM_POSTCODE," &
        '                    "CM_COUNTRY,CM_PHONE,CM_FAX," &
        '                    "CM_EMAIL,CM_BUSINESS_REG_NO,CM_ACCT_NO," &
        '                    "CM_BANK,CM_BRANCH,CM_CURRENCY_CODE," &
        '                    "CM_COY_LOGO,CM_ACTUAL_TERMSANDCONDFILE,CM_HUB_TERMSANDCONDFILE,CM_TAX_REG_NO,CM_LAST_DATE," &
        '                    "CM_TAX_CALC_BY,CM_BCM_SET,CM_FINDEPT_MODE, CM_INV_APPR," &
        '                    "CM_PAYMENT_TERM,CM_PAYMENT_METHOD,CM_PWD_DURATION," &
        '                    "CM_COY_LONG_NAME,CM_WEBSITE,CM_YEAR_REG,CM_PAIDCAPITAL_CURRENCY_CODE,CM_PAIDCAPITAL, " &
        '                    "CM_OWNERSHIP1,CM_OWNERSHIP2,CM_BUSINESS_NATURE,CM_COMMODITY,CM_REGORGCODE," &
        '                    "CM_SKINS_ID,CM_TRAINING,CM_PRIV_LABELING,CM_ENT_BY,CM_ENT_DT,CM_SKU,CM_TRANS_NO,CM_CONTACT,CM_REPORT_USERS,CM_MULTI_PO,CM_BA_CANCEL,CM_BANK_NAME) " &
        '            "VALUES('" & Common.Parse(pComp.CoyId) & "','" & Common.Parse(pComp.CoyName) & "','" & Common.Parse(pComp.Status) & "','" &
        '                    Common.Parse(pComp.CoyType) & "','" & Common.Parse(pComp.Package) & "'," &
        '                    Common.Parse(pComp.LicenseUsers) & "," & Common.ConvertDate(pComp.SubStart) & "," & Common.ConvertDate(pComp.SubEnd) & ",'" &
        '                    Common.Parse(pComp.Address1) & "','" & Common.Parse(pComp.Address2) & "','" & Common.Parse(pComp.Address3) & "','" &
        '                    Common.Parse(pComp.City) & "','" & Common.Parse(pComp.State) & "','" & Common.Parse(pComp.PostCode) & "','" &
        '                    Common.Parse(pComp.Country) & "','" & Common.Parse(pComp.Phone) & "','" & Common.Parse(pComp.Fax) & "','" &
        '                    Common.Parse(pComp.Email) & "','" & Common.Parse(pComp.BusinessRegNo) & "','" & Common.Parse(pComp.AccountNo) & "','" &
        '                    Common.Parse(pComp.BankCode) & "','" & Common.Parse(pComp.BranchCode) & "','" & Common.Parse(pComp.Currency) & "','" &
        '                    Common.Parse(pComp.CoyLogo) & "','" & Common.Parse(pComp.Actual_TC) & "','" & Common.Parse(pComp.TC) & "','" & Common.Parse(pComp.TaxRegNo) & "','" &
        '                    IIf(pComp.GSTDateLastStatus = "", "NULL", Common.ConvertDate(pComp.GSTDateLastStatus)) & "," &
        '                    Common.Parse(pComp.TaxCalBy) & "', 'l','" & Common.Parse(pComp.BCMSetting) & "','" & Common.Parse(pComp.FinDeptMode) & "','" & Common.Parse(pComp.InvAppr) & "','" &
        '                    Common.Parse(pComp.PaymentTerm) & "','" & Common.Parse(pComp.PaymentMethod) & "','" & Common.Parse(pComp.PwdDuration) & "','" &
        '                    Common.Parse(pComp.CoyLongName) & "','" & Common.Parse(pComp.WebSites) & "','" & Common.Parse(pComp.RegYear) & "','" &
        '                    Common.Parse(pComp.RegCurrency) & "'," & IIf(Common.Parse(pComp.PaidUpCapital) = "", "NULL", Common.Parse(pComp.PaidUpCapital)) & ",'" &
        '                    Common.Parse(pComp.Ownership) & "','" & Common.Parse(pComp.OwnershipOthers) & "','" &
        '                    Common.Parse(pComp.Business) & "','" & Common.Parse(pComp.Commodity) & "','" & Common.Parse(pComp.OrgCode) & "','" &
        '                    Common.Parse(pComp.Skins) & "','" & Common.Parse(pComp.TrainDemo) & "','" & Common.Parse(pComp.PrivLabeling) & "','" & ctx.Session("UserId") & "',GETDATE()," &
        '                    IIf(Common.Parse(pComp.SKU) = "", "NULL", Common.Parse(pComp.SKU)) & "," & IIf(Common.Parse(pComp.TransNo) = "", "NULL", Common.Parse(pComp.TransNo)) & ", '" &
        '                    Common.Parse(pComp.ContactPerson) & "','" & IIf(Common.Parse(pComp.ReportUsers) = "", 0, Common.Parse(pComp.ReportUsers)) & "','" & Common.Parse(pComp.MultiInvAppr) & "','" & Common.Parse(pComp.BACanPO) & "','" & Common.Parse(pComp.BankName) & "')"

        '            If objDb.Execute(strSql) Then
        '                AddCompParam(pComp)
        '                'Michelle (21/1/2011) - To set the Invoice Approval Rule to True if there the company is with Invoice Approval
        '                ' AddCompSetting(pComp)
        '                If Common.Parse(pComp.InvAppr) = "N" Then
        '                    AddCompSetting(pComp)
        '                Else
        '                    AddCompSetting(pComp, "Y")
        '                End If
        '                AddExchnageRate(pComp)

        '                If Common.Parse(pComp.CoyType) = "BOTH" Or Common.Parse(pComp.CoyType) = "BUYER" Then AddBCompDefault(Common.Parse(pComp.CoyId))

        '                If Not strApp Is Nothing Then
        '                    addAppPackage(pComp, strApp)
        '                End If

        '                Message = Common.RecordSave
        '                Return True
        '            End If
        '        End If
        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function
        Public Function AddCompanyByBilling(ByVal pComp As Company, ByVal UserID As String, Optional ByVal strApp() As String = Nothing, Optional ByVal strBuyerType As String = "") As Boolean
            AddCompanyByBilling = False

            Dim strSql, strSqlExist, strSqlDel As String
            If pComp.CoyType = "BUYER" Or pComp.CoyType = "BOTH" Then
                pComp.ContrPRSetting = "B"
            Else
                pComp.ContrPRSetting = ""
            End If


            strSqlExist = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
                  Common.Parse(pComp.CoyId) & "' AND CM_DELETED <>'Y'"

            strSqlDel = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
                   Common.Parse(pComp.CoyId) & "' AND CM_DELETED='Y'"
            Try

                If objDb.Exist(strSqlExist) Then
                    strCompMassage = Common.RecordDuplicate
                    Return False
                ElseIf objDb.Exist(strSqlDel) Then
                    strCompMassage = Common.RecordUsed
                    Return False
                Else
                    ' ai chu add on 05/12/2005
                    ' SR U30012 - to include additional fields to allow hub admin enter the 
                    ' no of SKU and no of transacrion at the company detail screen
                    strSql = "INSERT INTO COMPANY_MSTR(CM_COY_ID,CM_COY_NAME,CM_STATUS," &
                            "CM_COY_TYPE,CM_LICENCE_PACKAGE, " &
                            "CM_LICENSE_USERS,CM_SUB_START_DT,CM_SUB_END_DT," &
                            "CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3," &
                            "CM_CITY,CM_STATE,CM_POSTCODE," &
                            "CM_COUNTRY,CM_PHONE,CM_FAX," &
                            "CM_EMAIL,CM_BUSINESS_REG_NO,CM_ACCT_NO," &
                            "CM_BANK,CM_BRANCH,CM_CURRENCY_CODE," &
                            "CM_COY_LOGO,CM_ACTUAL_TERMSANDCONDFILE,CM_HUB_TERMSANDCONDFILE,CM_TAX_REG_NO," &
                            "CM_TAX_CALC_BY,CM_BCM_SET,CM_FINDEPT_MODE, CM_INV_APPR," &
                            "CM_PAYMENT_TERM,CM_PAYMENT_METHOD,CM_PWD_DURATION," &
                            "CM_COY_LONG_NAME,CM_WEBSITE,CM_YEAR_REG,CM_PAIDCAPITAL_CURRENCY_CODE,CM_PAIDCAPITAL, " &
                            "CM_OWNERSHIP1,CM_OWNERSHIP2,CM_BUSINESS_NATURE,CM_COMMODITY,CM_REGORGCODE," &
                            "CM_SKINS_ID,CM_TRAINING,CM_PRIV_LABELING,CM_ENT_BY,CM_ENT_DT,CM_SKU,CM_TRANS_NO,CM_CONTACT,CM_REPORT_USERS,CM_MULTI_PO,CM_BA_CANCEL,CM_BANK_NAME,CM_CONTR_PR_SETTING) " &
                    "VALUES('" & Common.Parse(pComp.CoyId) & "','" & Common.Parse(pComp.CoyName) & "','" & Common.Parse(pComp.Status) & "','" &
                            Common.Parse(pComp.CoyType) & "','" & Common.Parse(pComp.Package) & "'," &
                            Common.Parse(pComp.LicenseUsers) & "," & Common.ConvertDate(pComp.SubStart) & "," & Common.ConvertDate(pComp.SubEnd) & ",'" &
                            Common.Parse(pComp.Address1) & "','" & Common.Parse(pComp.Address2) & "','" & Common.Parse(pComp.Address3) & "','" &
                            Common.Parse(pComp.City) & "','" & Common.Parse(pComp.State) & "','" & Common.Parse(pComp.PostCode) & "','" &
                            Common.Parse(pComp.Country) & "','" & Common.Parse(pComp.Phone) & "','" & Common.Parse(pComp.Fax) & "','" &
                            Common.Parse(pComp.Email) & "','" & Common.Parse(pComp.BusinessRegNo) & "','" & Common.Parse(pComp.AccountNo) & "','" &
                            Common.Parse(pComp.BankCode) & "','" & Common.Parse(pComp.BranchCode) & "','" & Common.Parse(pComp.Currency) & "','" &
                            Common.Parse(pComp.CoyLogo) & "','" & Common.Parse(pComp.Actual_TC) & "','" & Common.Parse(pComp.TC) & "','" & Common.Parse(pComp.TaxRegNo) & "','" &
                            Common.Parse(pComp.TaxCalBy) & "', '" & Common.Parse(pComp.BCMSetting) & "','" & Common.Parse(pComp.FinDeptMode) & "','" & Common.Parse(pComp.InvAppr) & "','" &
                            Common.Parse(pComp.PaymentTerm) & "','" & Common.Parse(pComp.PaymentMethod) & "','" & Common.Parse(pComp.PwdDuration) & "','" &
                            Common.Parse(pComp.CoyLongName) & "','" & Common.Parse(pComp.WebSites) & "','" & Common.Parse(pComp.RegYear) & "','" &
                            Common.Parse(pComp.RegCurrency) & "'," & IIf(Common.Parse(pComp.PaidUpCapital) = "", "NULL", Common.Parse(pComp.PaidUpCapital)) & ",'" &
                            Common.Parse(pComp.Ownership) & "','" & Common.Parse(pComp.OwnershipOthers) & "','" &
                            Common.Parse(pComp.Business) & "','" & Common.Parse(pComp.Commodity) & "','" & Common.Parse(pComp.OrgCode) & "','" &
                            Common.Parse(pComp.Skins) & "','" & Common.Parse(pComp.TrainDemo) & "','" & Common.Parse(pComp.PrivLabeling) & "','" & UserID & "',GETDATE()," &
                            IIf(Common.Parse(pComp.SKU) = "", "NULL", Common.Parse(pComp.SKU)) & "," & IIf(Common.Parse(pComp.TransNo) = "", "NULL", Common.Parse(pComp.TransNo)) & ", '" &
                            Common.Parse(pComp.ContactPerson) & "','" & IIf(Common.Parse(pComp.ReportUsers) = "", 0, Common.Parse(pComp.ReportUsers)) & "','" & Common.Parse(pComp.MultiInvAppr) & "','" & Common.Parse(pComp.BACanPO) & "','" & Common.Parse(pComp.BankName) & "','" & Common.Parse(pComp.ContrPRSetting) & "')"

                    If objDb.Execute(strSql) Then
                        AddCompParam(pComp)
                        'Michelle (21/1/2011) - To set the Invoice Approval Rule to True if there the company is with Invoice Approval
                        ' AddCompSetting(pComp)
                        If Common.Parse(pComp.InvAppr) = "N" Then
                            AddCompSetting(pComp)
                        Else
                            AddCompSetting(pComp, "Y")
                        End If
                        'AddExchnageRateByBilling(pComp, UserID)

                        If Common.Parse(pComp.CoyType) = "BOTH" Or Common.Parse(pComp.CoyType) = "BUYER" Then AddBCompDefault(Common.Parse(pComp.CoyId), strBuyerType)

                        If Not strApp Is Nothing Then
                            addAppPackage(pComp, strApp)
                        End If

                        Message = Common.RecordSave
                        Return True
                    End If
                End If
            Catch errExp As CustomException
                Common.TrwExp(errExp)
            Catch errExp1 As Exception
                Common.TrwExp(errExp1)
            End Try
        End Function

        'Function UpdateCompany(ByVal pComp As Company, Optional ByVal strApp() As String = Nothing, Optional ByVal iseprocure As Boolean = True) As Boolean
        '    Dim strSql As String
        '    If iseprocure = True Then

        '        strSql = "UPDATE COMPANY_MSTR SET CM_COY_NAME='" & Common.Parse(pComp.CoyName) & "'," &
        '        "CM_COY_TYPE='" & Common.Parse(pComp.CoyType) & "'," &
        '        "CM_PARENT_COY_ID='" & Common.Parse(pComp.ParentCoy) & "'," &
        '        "CM_LICENCE_PACKAGE='" & Common.Parse(pComp.Package) & "', " &
        '        "CM_LICENSE_USERS='" & Common.Parse(pComp.LicenseUsers) & "', " &
        '        "CM_SUB_START_DT=" & Common.ConvertDate(pComp.SubStart) & ", " &
        '        "CM_SUB_END_DT=" & Common.ConvertDate(pComp.SubEnd) & ", " &
        '        "CM_ADDR_LINE1='" & Common.Parse(pComp.Address1) & "'," &
        '        "CM_ADDR_LINE2 ='" & Common.Parse(pComp.Address2) & "'," &
        '        "CM_ADDR_LINE3 ='" & Common.Parse(pComp.Address3) & "'," &
        '        "CM_CITY ='" & Common.Parse(pComp.City) & "'," &
        '        "CM_STATE='" & Common.Parse(pComp.State) & "'," &
        '        "CM_POSTCODE ='" & Common.Parse(pComp.PostCode) & "'," &
        '        "CM_COUNTRY ='" & Common.Parse(pComp.Country) & "'," &
        '        "CM_PHONE='" & Common.Parse(pComp.Phone) & "'," &
        '        "CM_FAX ='" & Common.Parse(pComp.Fax) & "'," &
        '        "CM_EMAIL ='" & Common.Parse(pComp.Email) & "'," &
        '        "CM_BUSINESS_REG_NO ='" & Common.Parse(pComp.BusinessRegNo) & "'," &
        '        "CM_ACCT_NO ='" & Common.Parse(pComp.AccountNo) & "'," &
        '        "CM_BANK='" & Common.Parse(pComp.BankCode) & "'," &
        '        "CM_BRANCH='" & Common.Parse(pComp.BranchCode) & "'," &
        '        "CM_CURRENCY_CODE='" & Common.Parse(pComp.Currency) & "', " &
        '        "CM_COY_LOGO='" & Common.Parse(pComp.CoyLogo) & "'," &
        '        "CM_HUB_TERMSANDCONDFILE='" & Common.Parse(pComp.TC) & "'," &
        '        "CM_ACTUAL_TERMSANDCONDFILE='" & Common.Parse(pComp.Actual_TC) & "'," &
        '        "CM_TAX_REG_NO ='" & Common.Parse(pComp.TaxRegNo) & "'," &
        '        "CM_LAST_DATE =" & IIf(pComp.GSTDateLastStatus = "", "NULL", Common.ConvertDate(pComp.GSTDateLastStatus)) & "," &
        '        "CM_TAX_CALC_BY='" & Common.Parse(pComp.TaxCalBy) & "'," &
        '        "CM_BCM_SET='" & Common.Parse(pComp.BCMSetting) & "'," &
        '        "CM_FINDEPT_MODE='" & Common.Parse(pComp.FinDeptMode) & "'," &
        '        "CM_INV_APPR='" & Common.Parse(pComp.InvAppr) & "'," &
        '        "CM_PAYMENT_TERM ='" & Common.Parse(pComp.PaymentTerm) & "'," &
        '        "CM_PAYMENT_METHOD ='" & Common.Parse(pComp.PaymentMethod) & "'," &
        '        "CM_PWD_DURATION=" & Common.Parse(pComp.PwdDuration) & "," &
        '        "CM_COY_LONG_NAME='" & Common.Parse(pComp.CoyLongName) & "'," &
        '        "CM_WEBSITE='" & Common.Parse(pComp.WebSites) & "'," &
        '        "CM_YEAR_REG='" & Common.Parse(pComp.RegYear) & "'," &
        '        "CM_PAIDCAPITAL_CURRENCY_CODE='" & Common.Parse(pComp.RegCurrency) & "'," &
        '        "CM_PAIDCAPITAL=" & IIf(Common.Parse(pComp.PaidUpCapital) = "", "NULL", Common.Parse(pComp.PaidUpCapital)) & "," &
        '        "CM_OWNERSHIP1='" & Common.Parse(pComp.Ownership) & "'," &
        '        "CM_OWNERSHIP2='" & Common.Parse(pComp.OwnershipOthers) & "'," &
        '        "CM_BUSINESS_NATURE='" & Common.Parse(pComp.Business) & "'," &
        '        "CM_COMMODITY='" & Common.Parse(pComp.Commodity) & "'," &
        '        "CM_REGORGCODE='" & Common.Parse(pComp.OrgCode) & "'," &
        '        "CM_PRIV_LABELING='" & Common.Parse(pComp.PrivLabeling) & "'," &
        '        "CM_SKINS_ID='" & Common.Parse(pComp.Skins) & "'," &
        '        "CM_TRAINING='" & Common.Parse(pComp.TrainDemo) & "'," &
        '        "CM_REPORT_USERS=" & IIf(Common.Parse(pComp.ReportUsers) = "", 0, Common.Parse(pComp.ReportUsers)) & "," &
        '        "CM_MOD_BY='" & ctx.Session("UserId") & "'," &
        '        "CM_MULTI_PO='" & Common.Parse(pComp.MultiInvAppr) & "'," &
        '        "CM_BA_CANCEL='" & Common.Parse(pComp.BACanPO) & "'," &
        '        "CM_BANK_NAME='" & Common.Parse(pComp.BankName) & "'," &
        '        "CM_MOD_DT=GETDATE()"
        '    Else
        '        strSql = "UPDATE COMPANY_MSTR SET CM_COY_NAME='" & Common.Parse(pComp.CoyName) & "'," &
        '        "CM_STATUS='" & Common.Parse(pComp.Status) & "'," &
        '        "CM_COY_TYPE='" & Common.Parse(pComp.CoyType) & "'," &
        '        "CM_PARENT_COY_ID='" & Common.Parse(pComp.ParentCoy) & "'," &
        '        "CM_LICENCE_PACKAGE='" & Common.Parse(pComp.Package) & "', " &
        '        "CM_LICENSE_USERS='" & Common.Parse(pComp.LicenseUsers) & "', " &
        '        "CM_SUB_START_DT=" & Common.ConvertDate(pComp.SubStart) & ", " &
        '        "CM_SUB_END_DT=" & Common.ConvertDate(pComp.SubEnd) & ", " &
        '        "CM_ADDR_LINE1='" & Common.Parse(pComp.Address1) & "'," &
        '        "CM_ADDR_LINE2 ='" & Common.Parse(pComp.Address2) & "'," &
        '        "CM_ADDR_LINE3 ='" & Common.Parse(pComp.Address3) & "'," &
        '        "CM_CITY ='" & Common.Parse(pComp.City) & "'," &
        '        "CM_STATE='" & Common.Parse(pComp.State) & "'," &
        '        "CM_POSTCODE ='" & Common.Parse(pComp.PostCode) & "'," &
        '        "CM_COUNTRY ='" & Common.Parse(pComp.Country) & "'," &
        '        "CM_PHONE='" & Common.Parse(pComp.Phone) & "'," &
        '        "CM_FAX ='" & Common.Parse(pComp.Fax) & "'," &
        '        "CM_EMAIL ='" & Common.Parse(pComp.Email) & "'," &
        '        "CM_BUSINESS_REG_NO ='" & Common.Parse(pComp.BusinessRegNo) & "'," &
        '        "CM_LAST_DATE =" & IIf(pComp.GSTDateLastStatus = "", "NULL", Common.ConvertDate(pComp.GSTDateLastStatus)) & "," &
        '        "CM_ACCT_NO ='" & Common.Parse(pComp.AccountNo) & "'," &
        '        "CM_BANK='" & Common.Parse(pComp.BankCode) & "'," &
        '        "CM_BRANCH='" & Common.Parse(pComp.BranchCode) & "'," &
        '        "CM_CURRENCY_CODE='" & Common.Parse(pComp.Currency) & "', " &
        '        "CM_COY_LOGO='" & Common.Parse(pComp.CoyLogo) & "'," &
        '        "CM_HUB_TERMSANDCONDFILE='" & Common.Parse(pComp.TC) & "'," &
        '        "CM_ACTUAL_TERMSANDCONDFILE='" & Common.Parse(pComp.Actual_TC) & "'," &
        '        "CM_TAX_REG_NO ='" & Common.Parse(pComp.TaxRegNo) & "'," &
        '        "CM_TAX_CALC_BY='" & Common.Parse(pComp.TaxCalBy) & "'," &
        '        "CM_BCM_SET='" & Common.Parse(pComp.BCMSetting) & "'," &
        '        "CM_FINDEPT_MODE='" & Common.Parse(pComp.FinDeptMode) & "'," &
        '        "CM_INV_APPR='" & Common.Parse(pComp.InvAppr) & "'," &
        '        "CM_PAYMENT_TERM ='" & Common.Parse(pComp.PaymentTerm) & "'," &
        '        "CM_PAYMENT_METHOD ='" & Common.Parse(pComp.PaymentMethod) & "'," &
        '        "CM_PWD_DURATION=" & Common.Parse(pComp.PwdDuration) & "," &
        '        "CM_COY_LONG_NAME='" & Common.Parse(pComp.CoyLongName) & "'," &
        '        "CM_WEBSITE='" & Common.Parse(pComp.WebSites) & "'," &
        '        "CM_REGORGCODE='" & Common.Parse(pComp.OrgCode) & "'," &
        '        "CM_PRIV_LABELING='" & Common.Parse(pComp.PrivLabeling) & "'," &
        '        "CM_SKINS_ID='" & Common.Parse(pComp.Skins) & "'," &
        '        "CM_TRAINING='" & Common.Parse(pComp.TrainDemo) & "'," &
        '        "CM_CONTACT='" & Common.Parse(pComp.ContactPerson) & "'," &
        '        "CM_REPORT_USERS=" & IIf(Common.Parse(pComp.ReportUsers) = "", 0, Common.Parse(pComp.ReportUsers)) & "," &
        '        "CM_MOD_BY='" & ctx.Session("UserId") & "'," &
        '        "CM_MULTI_PO='" & Common.Parse(pComp.MultiInvAppr) & "'," &
        '        "CM_BA_CANCEL='" & Common.Parse(pComp.BACanPO) & "'," &
        '        "CM_BANK_NAME='" & Common.Parse(pComp.BankName) & "'," &
        '        "CM_MOD_DT=GETDATE()"

        '    End If

        '    ' ai chu add on 05/12/2005
        '    ' SR U30012 - to include additional fields to allow hub admin enter the 
        '    ' no of SKU and no of transacrion at the company detail screen
        '    If pComp.CoyType = "VENDOR" Or pComp.CoyType = "BOTH" Then
        '        'strSql &= ", CM_SKU = " & pComp.SKU & ", CM_TRANS_NO = " & pComp.TransNo & " "
        '        strSql &= ", CM_SKU = " & IIf(Common.Parse(pComp.SKU) = "", "NULL", Common.Parse(pComp.SKU)) & ", CM_TRANS_NO = " & IIf(Common.Parse(pComp.TransNo) = "", "NULL", Common.Parse(pComp.TransNo)) & " "
        '    End If

        '    strSql &= "WHERE CM_COY_ID ='" & Common.Parse(pComp.CoyId) & "'"

        '    If objDb.Execute(strSql) Then
        '        EditCompParam(pComp)
        '        If Not strApp Is Nothing Then
        '            addAppPackage(pComp, strApp)
        '        End If

        '        Message = Common.RecordSave
        '        Return True
        '    End If
        'End Function


        Public Function GetCompanyDetails(ByVal pCompId As String) As Company
            Dim strGet As String
            Dim dtCoy As DataTable
            Dim objCoyDetails As New Company
            strGet = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Common.Parse(pCompId) & "'"
            dtCoy = objDb.FillDt(strGet)

            If Not dtCoy Is Nothing Then
                With dtCoy.Rows(0)
                    objCoyDetails.CoyId = IIf(IsDBNull(.Item("CM_COY_ID")), "", .Item("CM_COY_ID"))
                    objCoyDetails.CoyName = IIf(IsDBNull(.Item("CM_COY_NAME")), "", .Item("CM_COY_NAME"))
                    objCoyDetails.ParentCoy = IIf(IsDBNull(.Item("CM_PARENT_COY_ID")), "", .Item("CM_PARENT_COY_ID"))
                    objCoyDetails.AccountNo = IIf(IsDBNull(.Item("CM_ACCT_NO")), "", .Item("CM_ACCT_NO"))
                    objCoyDetails.BankCode = IIf(IsDBNull(.Item("CM_BANK")), "", .Item("CM_BANK"))
                    objCoyDetails.BranchCode = IIf(IsDBNull(.Item("CM_BRANCH")), "", .Item("CM_BRANCH"))
                    objCoyDetails.Currency = IIf(IsDBNull(.Item("CM_CURRENCY_CODE")), "", .Item("CM_CURRENCY_CODE"))
                    objCoyDetails.Address1 = IIf(IsDBNull(.Item("CM_ADDR_LINE1")), "", .Item("CM_ADDR_LINE1"))
                    objCoyDetails.Address2 = IIf(IsDBNull(.Item("CM_ADDR_LINE2")), "", .Item("CM_ADDR_LINE2"))
                    objCoyDetails.Address3 = IIf(IsDBNull(.Item("CM_ADDR_LINE3")), "", .Item("CM_ADDR_LINE3"))
                    objCoyDetails.City = IIf(IsDBNull(.Item("CM_CITY")), "", .Item("CM_CITY"))
                    objCoyDetails.State = IIf(IsDBNull(.Item("CM_STATE")), "", .Item("CM_STATE"))
                    objCoyDetails.PostCode = IIf(IsDBNull(.Item("CM_POSTCODE")), "", .Item("CM_POSTCODE"))
                    objCoyDetails.Country = IIf(IsDBNull(.Item("CM_COUNTRY")), "", .Item("CM_COUNTRY"))
                    objCoyDetails.Phone = IIf(IsDBNull(.Item("CM_PHONE")), "", .Item("CM_PHONE"))
                    objCoyDetails.Fax = IIf(IsDBNull(.Item("CM_FAX")), "", .Item("CM_FAX"))
                    objCoyDetails.SubStart = IIf(IsDBNull(.Item("CM_SUB_START_DT")), "", .Item("CM_SUB_START_DT"))
                    objCoyDetails.SubEnd = IIf(IsDBNull(.Item("CM_SUB_END_DT")), "", .Item("CM_SUB_END_DT"))
                    objCoyDetails.Email = IIf(IsDBNull(.Item("CM_EMAIL")), "", .Item("CM_EMAIL"))
                    objCoyDetails.CoyLogo = IIf(IsDBNull(.Item("CM_COY_LOGO")), "", .Item("CM_COY_LOGO"))
                    objCoyDetails.BusinessRegNo = IIf(IsDBNull(.Item("CM_BUSINESS_REG_NO")), "", .Item("CM_BUSINESS_REG_NO"))
                    objCoyDetails.TaxRegNo = IIf(IsDBNull(.Item("CM_TAX_REG_NO")), "", .Item("CM_TAX_REG_NO"))
                    objCoyDetails.GSTDateLastStatus = IIf(IsDBNull(.Item("CM_LAST_DATE")), "", .Item("CM_LAST_DATE"))
                    objCoyDetails.TaxCalBy = IIf(IsDBNull(.Item("CM_TAX_CALC_BY")), "", .Item("CM_TAX_CALC_BY"))
                    objCoyDetails.PaymentMethod = IIf(IsDBNull(.Item("CM_PAYMENT_METHOD")), "", .Item("CM_PAYMENT_METHOD"))
                    objCoyDetails.PaymentTerm = IIf(IsDBNull(.Item("CM_PAYMENT_TERM")), "", .Item("CM_PAYMENT_TERM"))
                    objCoyDetails.PwdDuration = IIf(IsDBNull(.Item("CM_PWD_DURATION")), "", .Item("CM_PWD_DURATION"))
                    objCoyDetails.CoyLongName = IIf(IsDBNull(.Item("CM_COY_LONG_NAME")), "", .Item("CM_COY_LONG_NAME"))
                    objCoyDetails.WebSites = IIf(IsDBNull(.Item("CM_WEBSITE")), "", .Item("CM_WEBSITE"))
                    objCoyDetails.RegYear = IIf(IsDBNull(.Item("CM_YEAR_REG")), "", .Item("CM_YEAR_REG"))
                    objCoyDetails.RegCurrency = IIf(IsDBNull(.Item("CM_PAIDCAPITAL_CURRENCY_CODE")), "", .Item("CM_PAIDCAPITAL_CURRENCY_CODE"))
                    objCoyDetails.PaidUpCapital = IIf(IsDBNull(.Item("CM_PAIDCAPITAL")), "", .Item("CM_PAIDCAPITAL"))
                    objCoyDetails.Ownership = IIf(IsDBNull(.Item("CM_OWNERSHIP1")), "", .Item("CM_OWNERSHIP1"))
                    objCoyDetails.OwnershipOthers = IIf(IsDBNull(.Item("CM_OWNERSHIP2")), "", .Item("CM_OWNERSHIP2"))
                    objCoyDetails.Business = IIf(IsDBNull(.Item("CM_BUSINESS_NATURE")), "", .Item("CM_BUSINESS_NATURE"))
                    objCoyDetails.Commodity = IIf(IsDBNull(.Item("CM_COMMODITY")), "", .Item("CM_COMMODITY"))
                    objCoyDetails.OrgCode = IIf(IsDBNull(.Item("CM_REGORGCODE")), "", .Item("CM_REGORGCODE"))
                    objCoyDetails.Status = IIf(IsDBNull(.Item("CM_STATUS")), "", .Item("CM_STATUS"))
                    objCoyDetails.CoyType = IIf(IsDBNull(.Item("CM_COY_TYPE")), "", .Item("CM_COY_TYPE"))
                    objCoyDetails.BCMSetting = IIf(IsDBNull(.Item("CM_BCM_SET")), "", .Item("CM_BCM_SET"))
                    objCoyDetails.BCMStart = IIf(IsDBNull(.Item("CM_BUDGET_FROM_DATE")), "", .Item("CM_BUDGET_FROM_DATE"))
                    objCoyDetails.BCMEnd = IIf(IsDBNull(.Item("CM_BUDGET_TO_DATE")), "", .Item("CM_BUDGET_TO_DATE"))
                    objCoyDetails.FinDeptMode = IIf(IsDBNull(.Item("CM_FINDEPT_MODE")), "", .Item("CM_FINDEPT_MODE"))
                    objCoyDetails.InvAppr = IIf(IsDBNull(.Item("CM_INV_APPR")), "", .Item("CM_INV_APPR"))
                    objCoyDetails.PrivLabeling = IIf(IsDBNull(.Item("CM_PRIV_LABELING")), "", .Item("CM_PRIV_LABELING"))
                    objCoyDetails.TrainDemo = IIf(IsDBNull(.Item("CM_TRAINING")), "", .Item("CM_TRAINING"))
                    objCoyDetails.Skins = IIf(IsDBNull(.Item("CM_SKINS_ID")), "", .Item("CM_SKINS_ID"))
                    objCoyDetails.Actual_TC = IIf(IsDBNull(.Item("CM_ACTUAL_TERMSANDCONDFILE")), "", .Item("CM_ACTUAL_TERMSANDCONDFILE"))
                    objCoyDetails.TC = IIf(IsDBNull(.Item("CM_HUB_TERMSANDCONDFILE")), "", .Item("CM_HUB_TERMSANDCONDFILE"))
                    objCoyDetails.LicenseUsers = IIf(IsDBNull(.Item("CM_LICENSE_USERS")), "", .Item("CM_LICENSE_USERS"))
                    objCoyDetails.Package = IIf(IsDBNull(.Item("CM_LICENCE_PACKAGE")), "", .Item("CM_LICENCE_PACKAGE"))
                    objCoyDetails.SKU = IIf(IsDBNull(.Item("CM_SKU")), "", .Item("CM_SKU"))
                    objCoyDetails.TransNo = IIf(IsDBNull(.Item("CM_TRANS_NO")), "", .Item("CM_TRANS_NO"))
                    objCoyDetails.ContactPerson = Common.parseNull(.Item("CM_CONTACT"))
                    objCoyDetails.ReportUsers = IIf(IsDBNull(.Item("CM_REPORT_USERS")), "", .Item("CM_REPORT_USERS"))
                    objCoyDetails.MultiInvAppr = IIf(IsDBNull(.Item("CM_MULTI_PO")), "", .Item("CM_MULTI_PO"))
                    objCoyDetails.BACanPO = IIf(IsDBNull(.Item("CM_BA_CANCEL")), "", .Item("CM_BA_CANCEL"))
                    objCoyDetails.BankName = IIf(IsDBNull(.Item("CM_BANK_NAME")), "", .Item("CM_BANK_NAME"))
                End With
                GetCompanyDetails = objCoyDetails
            Else
                GetCompanyDetails = Nothing
            End If

        End Function

        Public Function GetCompanyType(ByVal companyID As String, Optional ByVal strCompany As String = "") As String
            Dim strGet As String
            Dim dsCoy As DataSet
            If strCompany = "" Then
                strCompany = companyID
            End If
            strGet = "SELECT CM_COY_TYPE FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCompany & "' "

            dsCoy = objDb.FillDs(strGet)
            If dsCoy.Tables(0).Rows.Count > 0 Then
                GetCompanyType = dsCoy.Tables(0).Rows(0).Item("CM_COY_TYPE")
            End If
        End Function
        Public Function GetCompanyDeletedStatus(ByVal sCompanyName As String) As String
            GetCompanyDeletedStatus = objDb.Get1Column("COMPANY_MSTR", "CM_DELETED", " WHERE CM_COY_NAME = '" & Replace(sCompanyName, "'", "''") & "' ")
        End Function


        Public Function GetCompanyName(ByVal strCoyID) As String
            Dim strGet As String
            Dim dsCoy As DataSet

            If strCoyID = "" Then
                strCoyID = HttpContext.Current.Session("CompanyID")
            End If
            strGet = "SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = '" &
            strCoyID & "'"

            dsCoy = objDb.FillDs(strGet)
            If dsCoy.Tables(0).Rows.Count > 0 Then
                GetCompanyName = dsCoy.Tables(0).Rows(0).Item("CM_COY_NAME")
            End If
        End Function

        Public Function getviewsubreq(ByVal strtypeofreq As String, ByVal strstatus As String, ByVal strdatetime As String) As DataSet
            Dim strget As String
            Dim dsview As DataSet

            strget = "SELECT * FROM VENDOR_REQUEST WHERE VR_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"

            'strget &= "VR_REQ_CODE ='" & Common.Parse(strcode) & "' "

            If strtypeofreq <> "" Then
                strget = strget & " AND VR_REQ_CATEGORY" & Common.ParseSQL(strtypeofreq)

                'strget &= " AND VR_REQ_CATEGORY = '" & Common.Parse(strtypeofreq) & "' "
            End If

            If strstatus <> "" Then
                strget = strget & " AND VR_STATUS" & Common.ParseSQL(strstatus)

                'strget &= " AND VR_STATUS = '" & Common.ParseSQL(strstatus) & "' "
            End If

            If strdatetime <> "" Then
                strget &= " AND VR_ENT_DATETIME BETWEEN "
                '(CONVERT("Date" & "00:00:00") AND CONVERT("Date" & "23:59:59")) = " & Common.ConvertDate(strdatetime) & " "
                'strget &= "'" & Common.ConvertDate(strdatetime) & " 00:00:00.000' AND "
                'strget &= "'" & Common.ConvertDate(strdatetime) & " 23:59:59.000' "
                strget &= "" & Common.ConvertDate(strdatetime & " 00:00:00.000") & " AND " & Common.ConvertDate(strdatetime & " 23:59:59.000") & ""
            End If

            'strget &= "AND VR_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            'strget &= "AND VR_ENT_BY = '" & HttpContext.Current.Session("UserId") & "' "

            dsview = objDb.FillDs(strget)
            getviewsubreq = dsview

        End Function

        'Name       : SearchCompany
        'Author     : kk
        'Descption  : Search all undelete company
        'LastUpadte : 22/11/2004
        Public Function SearchCompany(ByVal pCompID As String, ByVal pCompName As String) As DataSet
            Dim strsql_Comp As String
            Dim dsUser As DataSet
            Dim strTemp As String
            strsql_Comp = "SELECT * FROM COMPANY_MSTR WHERE   "

            If pCompID <> "" Then
                strTemp = Common.BuildWildCard(pCompID)
                strsql_Comp = strsql_Comp & " UPPER(CM_COY_ID)" & Common.ParseSQL(strTemp) & "AND "
            End If

            If pCompName <> "" Then
                strTemp = Common.BuildWildCard(pCompName)
                strsql_Comp = strsql_Comp & " UPPER(CM_COY_NAME)" & Common.ParseSQL(strTemp) & "AND "
            End If
            strsql_Comp &= "CM_DELETED='N'"

            dsUser = objDb.FillDs(strsql_Comp)
            SearchCompany = dsUser
        End Function

        'Name       : DeleteCompany
        'Author     : kk
        'Descption  : delete company
        'LastUpadte : 26/11/2004
        'Public Function DeleteCompany(ByVal pCompId As String) As Boolean
        '    Dim strdelete As String
        '    Dim dvComp As DataView
        '    Dim objUser As New Users
        '    Dim objComp As New Companies

        '    Try

        '        If objUser.GetUserCount(pCompId, True) > 0 Then
        '            strCompMassage = Common.RecordUserCascade
        '            Return False
        '        ElseIf objComp.CountCompanyVendor(pCompId) > 0 Then
        '            strCompMassage = Common.RecordVendorCascade
        '            Return False
        '        Else
        '            strdelete = "UPDATE COMPANY_MSTR SET CM_DELETED='Y', " &
        '                            "CM_MOD_BY='" & ctx.Session("UserId") & "', " &
        '                            "CM_MOD_DT=GETDATE() " &
        '                            "WHERE CM_COY_ID='" & pCompId & "'"

        '            If objDb.Execute(strdelete) Then
        '                strCompMassage = Common.RecordDelete
        '                Return True
        '            Else
        '                strCompMassage = Common.RecordNotDelete
        '                Return False
        '            End If
        '        End If

        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        Public Function CountCompanyVendor(ByVal pCompId As String) As Integer
            Dim iCnt As Integer
            Dim strSQL As String
            'SQL to check user limit
            strSQL = "SELECT COUNT(*) FROM COMPANY_VENDOR WHERE " &
                     "CV_S_COY_ID='" & Common.Parse(pCompId) & "'"
            iCnt = objDb.GetVal(strSQL)
            Return iCnt

        End Function
        'Name       : DeleteCompany
        'Author     : kk
        'Descption  : delete company
        'LastUpadte : 26/11/2004
        'Public Function ActivateCompany(ByVal pCompId As String, ByVal pBoo As Boolean) As Boolean
        '    Dim strUpdate As String
        '    Dim dvComp As DataView
        '    Dim cInd As Char
        '    cInd = IIf(pBoo, "A", "I")
        '    Try
        '        strUpdate = "UPDATE COMPANY_MSTR SET CM_STATUS='" & cInd & "'," &
        '        "CM_MOD_BY='" & ctx.Session("UserId") & "', " &
        '        "CM_MOD_DT=" & Common.ConvertDate(Now) &
        '        " WHERE CM_COY_ID='" & pCompId & "'"

        '        If objDb.Execute(strUpdate) Then
        '            Return True
        '        Else
        '            Return True
        '        End If
        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        'Public Function SearchCompany() As DataView
        '    Dim strGet As String
        '    Dim dvComp As DataView
        '    strGet = "SELECT * FROM SearchCompany"
        '    dvComp = objDb.GetView(strGet)
        '    SearchCompany = dvComp
        'End Function

        Public Function GetAllCompany() As DataView
            Dim strGet As String
            Dim dvComp As DataView
            strGet = "SELECT * FROM COMPANY_MSTR WHERE CM_DELETED='N' ORDER BY CM_COY_NAME"
            dvComp = objDb.GetView(strGet)
            GetAllCompany = dvComp
        End Function

        Public Function GetCompanyID(ByVal sCompanyName As String) As String
            GetCompanyID = objDb.Get1Column("COMPANY_MSTR", "CM_COY_ID", " WHERE CM_COY_NAME = '" & Replace(sCompanyName, "'", "''") & "' ")
        End Function

        'Public Function GetAllDept() As DataView
        '    Dim strGet As String
        '    Dim dvDept As DataView
        '    strGet = "SELECT * FROM COMPANY_DEPT_MSTR WHERE CDM_COY_ID='" & ctx.Session("CompanyId") & "'"
        '    dvDept = objDb.GetView(strGet)
        '    GetAllDept = dvDept
        'End Function

        Public Function SearchDept(ByVal strDeptCode As String, ByVal strDeptName As String) As DataSet
            Dim strsql_sdept As String
            Dim dssdept As DataSet
            strsql_sdept = "select CDM_DEPT_CODE, CDM_DEPT_NAME from COMPANY_DEPT_MSTR WHERE CDM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND CDM_DELETED='N'"

            If strDeptCode <> "" Then
                strsql_sdept = strsql_sdept & " AND CDM_DEPT_CODE" & Common.ParseSQL(strDeptCode)
            End If

            If strDeptName <> "" Then
                strsql_sdept = strsql_sdept & " AND CDM_DEPT_NAME" & Common.ParseSQL(strDeptName)
            End If

            dssdept = objDb.FillDs(strsql_sdept)
            SearchDept = dssdept
        End Function

        'Public Function DelUsrLocation(ByVal pUserId As String, ByVal pLevel As Integer, Optional ByVal pAddr As String = "") As Boolean
        '    Dim strSQL As String
        '    strSQL = "DELETE FROM USERS_LOCATION WHERE UL_COY_ID='" & ctx.Session("CompanyId") & "' AND UL_USER_ID='" & pUserId & "' AND UL_LEVEL=" & pLevel

        '    If pAddr <> "" Then
        '        strSQL &= " AND UL_ADDR_CODE IN (" & pAddr & ")"
        '    End If
        '    objDb.Execute(strSQL)
        'End Function

        'Public Function DelFinDeptViewing(ByVal pUserId As String, Optional ByVal pDept As String = "", Optional ByVal pAll As Boolean = False) As Boolean
        '    Dim strSQL As String
        '    strSQL = "DELETE FROM FINANCE_USER_DEPARTMENT WHERE FUD_COY_ID='" & ctx.Session("CompanyId") & "' AND FUD_USER_ID='" & pUserId & "'"


        '    If pDept <> "" Then
        '        strSQL &= " AND FUD_DEPT_CODE IN (" & pDept & ") "
        '    End If
        '    If pAll Then
        '        strSQL &= " AND FUD_VIEWOPTION='1'"
        '    End If
        '    objDb.Execute(strSQL)
        'End Function

        'Public Function GetFinMode(ByVal pHubLevel As Boolean) As String
        '    Dim strSQL, strCompId As String

        '    If pHubLevel Then
        '        strCompId = ctx.Session("CompanyIdToken")
        '    Else
        '        strCompId = ctx.Session("CompanyId")
        '    End If
        '    strSQL = "SELECT CM_FINDEPT_MODE FROM COMPANY_MSTR WHERE CM_COY_ID='" & strCompId & "'"
        '    Return objDb.GetVal(strSQL)

        'End Function

        Public Function GetInvApprMode(ByVal pCompanyId As String) As String
            Dim strSQL As String

            strSQL = "SELECT CM_INV_APPR FROM COMPANY_MSTR WHERE CM_COY_ID='" & pCompanyId & "'"
            Return objDb.GetVal(strSQL)

        End Function
        Public Function GetMultiInvApprMode(ByVal pCompanyId As String) As String
            Dim strSQL As String
            strSQL = "SELECT CM_MULTI_PO FROM COMPANY_MSTR WHERE CM_COY_ID='" & pCompanyId & "'"
            Return objDb.GetVal(strSQL)
        End Function
        Public Function GetBACanPOMode(ByVal pCompanyId As String) As String
            Dim strSQL As String
            strSQL = "SELECT CM_BA_CANCEL FROM COMPANY_MSTR WHERE CM_COY_ID='" & pCompanyId & "'"
            Return objDb.GetVal(strSQL)
        End Function

        'Public Function GetPwdDuration(ByVal pHubLevel As Boolean) As Integer
        '    Dim strSQL, strCompId As String

        '    If pHubLevel Then
        '        strCompId = ctx.Session("CompanyIdToken")
        '    Else
        '        strCompId = ctx.Session("CompanyId")
        '    End If
        '    strSQL = "SELECT CM_PWD_DURATION FROM COMPANY_MSTR WHERE CM_COY_ID='" & strCompId & "'"
        '    Return objDb.GetVal(strSQL)

        'End Function

        Public Function getAppPackage(ByVal strCoyID As String)
            Dim strsql As String
            Dim ds As DataSet

            strsql = " select ap_app_id, ap_app_name, 'Y' as chk from COMPANY_APPLICATION  "
            strsql &= "left join application_mstr on ap_app_id=ca_app_id where CA_COY_ID = '" & Common.Parse(strCoyID) & "' "
            strsql &= "union select ap_app_id, ap_app_name, 'N' as chk "
            strsql &= "from application_mstr where ap_app_id not in "
            strsql &= "(select ca_app_id from COMPANY_APPLICATION where CA_COY_ID  = '" & Common.Parse(strCoyID) & "')"

            ds = objDb.FillDs(strsql)
            getAppPackage = ds
        End Function

        Public Function GetPwdDuration(ByVal pCompId As String) As Integer
            Dim objDcom As New EAD.DBCom
            Dim strSQL, strCompId As String
            strSQL = "SELECT CM_PWD_DURATION FROM COMPANY_MSTR WHERE CM_COY_ID='" & pCompId & "'"
            Dim dt As DataTable = objDcom.FillDt(strSQL)
            If dt.Rows.Count > 0 Then
                Return CStr(dt.Rows(0).Item("CM_PWD_DURATION"))
            Else
                Return 0
            End If

            'Return objDb.GetVal(strSQL)
        End Function

        Public Function AddCompParam(ByVal pComp As Company) As Boolean
            Dim strSQL, strCompId, Query(0) As String
            Dim i, j As Integer
            Dim sParamName() As String = {"Prefix", "Last Used No"}
            Dim sCompVendor() As String = {"Quotation", "DO", "CSR", "Invoice"}
            Dim sCompVendorVal() As String = {"QTN", "DO", "", "INV"}
            'Dim sCompBuyer() As String = {"RFQ", "PR", "PO", "GRN", "SRN", "Payment", "CR"}
            'Dim sCompBuyerVal() As String = {"RFQ", "", "PO", "GRN", "", "PY", "CR"}

            'Modified by Joon on 06 Apr 2011
            Dim sCompBuyer() As String = {"RFQ", "PR", "PO", "GRN", "SRN", "Payment", "CR", "IR", "IT"}
            Dim sCompBuyerVal() As String = {"RFQ", "PR", "PO", "GRN", "", "PY", "CR", "IR", "IT"}

            If pComp.CoyType = "VENDOR" Or pComp.CoyType = "BOTH" Then
                For i = 0 To sParamName.Length - 1
                    For j = 0 To sCompVendor.Length - 1
                        strSQL = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_NAME,CP_PARAM_TYPE,CP_APP_PKG, CP_PARAM_VALUE) " &
                                 "VALUES ('" &
                                 Common.Parse(pComp.CoyId) & "','" &
                                  Common.Parse(sParamName(i)) & "','" &
                                 Common.Parse(sCompVendor(j)) & "','eProcure','"
                        Select Case Common.Parse(sParamName(i))
                            Case "Prefix"
                                strSQL &= Common.Parse(sCompVendorVal(j)) & "')"
                            Case "Last Used No"
                                If Common.Parse(sCompVendorVal(j)) = "" Then
                                    strSQL &= "')"
                                Else
                                    strSQL &= "0000000')"
                                End If
                        End Select
                        Common.Insert2Ary(Query, strSQL)
                    Next
                Next
            End If
            If pComp.CoyType = "BUYER" Or pComp.CoyType = "BOTH" Then
                For i = 0 To sParamName.Length - 1
                    For j = 0 To sCompBuyer.Length - 1
                        strSQL = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_NAME,CP_PARAM_TYPE,CP_APP_PKG, CP_PARAM_VALUE) " &
                                 "VALUES ('" &
                                 Common.Parse(pComp.CoyId) & "','" &
                                 Common.Parse(sParamName(i)) & "','" &
                                 Common.Parse(sCompBuyer(j)) & "','eProcure','"
                        Select Case Common.Parse(sParamName(i))
                            Case "Prefix"
                                strSQL &= Common.Parse(sCompBuyerVal(j)) & "')"
                            Case "Last Used No"
                                If Common.Parse(sCompBuyerVal(j)) = "" Then
                                    strSQL &= "')"
                                Else
                                    strSQL &= "0000000')"
                                End If
                        End Select
                        Common.Insert2Ary(Query, strSQL)
                    Next
                Next
            End If

            If objDb.BatchExecute(Query) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function EditCompParam(ByVal pComp As Company) As Boolean
            Dim strSQL, strCompId, Query(0) As String
            Dim i, j As Integer
            Dim ds As DataSet
            Dim sParamName() As String = {"Prefix", "Last Used No"}
            Dim sCompVendor() As String = {"Quotation", "DO", "CSR", "Invoice"}
            Dim sCompVendorVal() As String = {"QTN", "DO", "", "INV"}
            Dim intInsert As Integer = 0

            'Modified by Joon on 06 Apr 2011
            Dim sCompBuyer() As String = {"RFQ", "PR", "PO", "GRN", "SRN", "Payment", "CR", "IR", "IT"}
            Dim sCompBuyerVal() As String = {"RFQ", "", "PO", "GRN", "", "PY", "CR", "IR", "IT"}

            If pComp.CoyType = "VENDOR" Or pComp.CoyType = "BOTH" Then
                For i = 0 To sParamName.Length - 1
                    For j = 0 To sCompVendor.Length - 1
                        strSQL = "SELECT * FROM company_param " _
                                & "WHERE CP_COY_ID='" & Common.Parse(pComp.CoyId) & "' " _
                                & "AND CP_PARAM_TYPE='" & Common.Parse(sCompVendor(j)) & "' " _
                                & "AND CP_PARAM_NAME='" & Common.Parse(sParamName(i)) & "'"
                        ds = objDb.FillDs(strSQL)
                        If ds.Tables(0).Rows.Count = 0 Then 'If record not found
                            Select Case Common.Parse(sParamName(i))
                                Case "Prefix"
                                    strSQL = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_NAME,CP_PARAM_TYPE,CP_APP_PKG, CP_PARAM_VALUE) " &
                                             "VALUES ('" &
                                             Common.Parse(pComp.CoyId) & "','" &
                                             Common.Parse(sParamName(i)) & "','" &
                                             Common.Parse(sCompVendor(j)) & "','eProcure','" &
                                             Common.Parse(sCompVendorVal(j)) & "')"

                                Case "Last Used No"
                                    strSQL = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_NAME,CP_PARAM_TYPE,CP_APP_PKG, CP_PARAM_VALUE) " &
                                             "VALUES ('" &
                                             Common.Parse(pComp.CoyId) & "','" &
                                             Common.Parse(sParamName(i)) & "','" &
                                             Common.Parse(sCompVendor(j)) & "','eProcure','"
                                    If Common.Parse(sCompVendorVal(j)) = "" Then
                                        strSQL &= "')"
                                    Else
                                        strSQL &= "0000000')"
                                    End If
                            End Select
                            Common.Insert2Ary(Query, strSQL)
                            intInsert = intInsert + 1
                        End If
                    Next
                Next
            End If

            If pComp.CoyType = "BUYER" Or pComp.CoyType = "BOTH" Then
                For i = 0 To sParamName.Length - 1
                    For j = 0 To sCompBuyer.Length - 1
                        strSQL = "SELECT * FROM company_param " _
                                & "WHERE CP_COY_ID='" & Common.Parse(pComp.CoyId) & "' " _
                                & "AND CP_PARAM_TYPE='" & Common.Parse(sCompBuyer(j)) & "' " _
                                & "AND CP_PARAM_NAME='" & Common.Parse(sParamName(i)) & "'"
                        ds = objDb.FillDs(strSQL)
                        If ds.Tables(0).Rows.Count = 0 Then 'If record not found
                            Select Case Common.Parse(sParamName(i))
                                Case "Prefix"
                                    strSQL = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_NAME,CP_PARAM_TYPE,CP_APP_PKG, CP_PARAM_VALUE) " &
                                             "VALUES ('" &
                                             Common.Parse(pComp.CoyId) & "','" &
                                             Common.Parse(sParamName(i)) & "','" &
                                             Common.Parse(sCompBuyer(j)) & "','eProcure','" &
                                             Common.Parse(sCompBuyerVal(j)) & "')"
                                Case "Last Used No"
                                    strSQL = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_NAME,CP_PARAM_TYPE,CP_APP_PKG, CP_PARAM_VALUE) " &
                                             "VALUES ('" &
                                             Common.Parse(pComp.CoyId) & "','" &
                                             Common.Parse(sParamName(i)) & "','" &
                                             Common.Parse(sCompBuyer(j)) & "','eProcure','"
                                    If Common.Parse(sCompBuyerVal(j)) = "" Then
                                        strSQL &= "')"
                                    Else
                                        strSQL &= "0000000')"
                                    End If
                            End Select
                            Common.Insert2Ary(Query, strSQL)
                            intInsert = intInsert + 1
                        End If
                    Next
                Next
            End If

            If intInsert > 0 Then
                If objDb.BatchExecute(Query) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return True
            End If

        End Function

        Public Function AddCompSetting(ByVal pComp As Company, Optional ByVal pInvAppr As String = "") As Boolean
            Dim strSQL, strCompId, Query(0) As String
            Dim i, j As Integer
            Dim sFlagName() As String = {"Buy Fixed Price", "Buy Discount Price", "Buy Contract Price", "Allow Free Form Billing Address", "Consolidation Required", "2 Level Receiving", "Approval Rule"}
            '//add by Moo
            Dim sFlagValue() As String = {"0", "0", "0", "0", "0", "0", "B+C"}


            If pComp.CoyType = "BUYER" Then
                For i = 0 To sFlagName.Length - 1
                    strSQL = "INSERT INTO COMPANY_SETTING(CS_COY_ID,CS_FLAG_NAME,CS_FLAG_VALUE,CS_FLAG_TYPE,CS_APP_PKG) " &
                             "VALUES ('" &
                             Common.Parse(pComp.CoyId) & "','" &
                             Common.Parse(sFlagName(i)) & "','" &
                             Common.Parse(sFlagValue(i)) & "','" &
                             IIf(i <= 2, "BuyActivity", "CoyParam") & "','eProcure')"
                    Common.Insert2Ary(Query, strSQL)

                Next
                If pInvAppr = "Y" Then
                    strSQL = "INSERT INTO COMPANY_SETTING(CS_COY_ID,CS_FLAG_NAME,CS_FLAG_VALUE,CS_FLAG_TYPE,CS_APP_PKG) " &
                             "VALUES ('" &
                             Common.Parse(pComp.CoyId) & "'," &
                             "'Invoice Approval Rule'," &
                             "'True'," &
                             "'CoyParam','eProcure')"
                    Common.Insert2Ary(Query, strSQL)
                End If
            End If

            If pComp.CoyType = "BUYER" AndAlso objDb.BatchExecute(Query) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function AddExchnageRate(ByVal pComp As Company) As Boolean
            Dim strSQL, strCompId As String
            strSQL = "INSERT INTO COMPANY_EXCHANGERATE(CE_COY_ID,CE_CURRENCY_CODE,CE_RATE,CE_ENT_BY,CE_ENT_DATETIME) " _
            & "SELECT '" & Common.Parse(pComp.CoyId) & "',CODE_ABBR,CODE_VALUE,'" &
            HttpContext.Current.Session("UserId") & "'," & Common.ConvertDate(Now) &
            " FROM CODE_MSTR WHERE CODE_CATEGORY='CU' AND CODE_DELETED='N'"

            If objDb.Execute(strSQL) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function AddExchnageRateByBilling(ByVal pComp As Company, ByVal UserID As String) As Boolean
            Dim strSQL, strCompId As String
            strSQL = "INSERT INTO COMPANY_EXCHANGERATE(CE_COY_ID,CE_CURRENCY_CODE,CE_RATE,CE_ENT_BY,CE_ENT_DATETIME) " _
            & "SELECT '" & Common.Parse(pComp.CoyId) & "',CODE_ABBR,CODE_VALUE,'" &
            UserID & "'," & Common.ConvertDate(Now) &
            " FROM CODE_MSTR WHERE CODE_CATEGORY='CU' AND CODE_DELETED='N'"

            If objDb.Execute(strSQL) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function AddBCompDefault(ByVal pCoyID As String, Optional ByVal strBuyerType As String = "") As Boolean
            Dim strSQL, Query(0) As String
            Dim objDb1 As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
            'Add Default Purchaser Catalogue
            strSQL = "INSERT INTO buyer_catalogue_mstr ("
            strSQL &= "BCM_B_COY_ID, BCM_GRP_DESC, BCM_ENT_BY, BCM_ENT_DATETIME)"
            strSQL &= "VALUES ('" & Common.Parse(pCoyID) & "', 'Default Purchaser Catalogue', "
            strSQL &= "'HUB', Now())"
            Common.Insert2Ary(Query, strSQL)

            'Add Default Approval Group (PM to AO)
            strSQL = "INSERT INTO APPROVAL_GRP_MSTR (AGM_GRP_NAME, AGM_COY_ID, AGM_ENT_BY, AGM_ENT_DATETIME, AGM_TYPE) "
            strSQL &= "VALUES ('Purchasing Manager to Approving Officer Approval', '" & Common.Parse(pCoyID) & "', "
            strSQL &= "'HUB', Now(), 'PO')"
            Common.Insert2Ary(Query, strSQL)

            'Add Default Approval Group (PO to PM)
            'Michelle (10/5/2012) - Issue 854
            Dim strBType As String
            ' yAP: 06Jul2012: strBuyerType = "1" = "FTN" = "S"
            If strBuyerType = "S" Then
                strBType = "Purchasing Officer to Purchasing Manager Approval"
            Else
                strBType = "Purchasing Officer to Approving Officer Approval"
            End If
            strSQL = "INSERT INTO APPROVAL_GRP_MSTR (AGM_GRP_NAME, AGM_COY_ID, AGM_ENT_BY, AGM_ENT_DATETIME, AGM_TYPE) "
            strSQL &= "VALUES ('" & strBType & "', '" & Common.Parse(pCoyID) & "', "
            strSQL &= "'HUB', Now(), 'PO')"
            Common.Insert2Ary(Query, strSQL)

            If strBuyerType = "S" Then
                'Michelle (10/9/2011) - To add in the default PR approval group
                strSQL = "INSERT INTO APPROVAL_GRP_MSTR (AGM_GRP_NAME, AGM_COY_ID, AGM_ENT_BY, AGM_ENT_DATETIME, AGM_TYPE) "
                strSQL &= "VALUES ('Buyer to Purchasing Manager Approval', '" & Common.Parse(pCoyID) & "', "
                strSQL &= "'HUB', Now(), 'PR')"
                Common.Insert2Ary(Query, strSQL)
            End If

            strSQL = "INSERT INTO APPROVAL_GRP_MSTR (AGM_GRP_NAME, AGM_COY_ID, AGM_ENT_BY, AGM_ENT_DATETIME, AGM_TYPE) "
            strSQL &= "VALUES ('Buyer to Approving Officer Approval', '" & Common.Parse(pCoyID) & "', "
            strSQL &= "'HUB', Now(), 'PR')"
            Common.Insert2Ary(Query, strSQL)


            If objDb1.BatchExecute(Query) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function addAppPackage(ByVal pComp As Company, ByVal strApp() As String) As Boolean
            Dim strSQL, strCompId, Query(0) As String
            Dim i As Integer

            strSQL = "delete from COMPANY_APPLICATION where CA_COY_ID = '" & Common.Parse(pComp.CoyId) & "'"
            Common.Insert2Ary(Query, strSQL)

            For i = 0 To strApp.Length - 1
                If strApp(i) <> "" Then

                    strSQL = "INSERT INTO COMPANY_APPLICATION(CA_COY_ID,CA_APP_ID) " &
                            "VALUES ('" &
                                   Common.Parse(pComp.CoyId) & "','" &
                                                 Common.Parse(strApp(i)) & "') "
                    Common.Insert2Ary(Query, strSQL)
                End If

            Next

            ' Remove user's user group data which application removed
            strSQL = "DELETE FROM USERS_USRGRP" &
                " WHERE EXISTS (" &
                " SELECT 1 FROM USER_GROUP_MSTR WHERE UU_USRGRP_ID = UGM_USRGRP_ID AND UU_APP_PKG = UGM_APP_PKG AND" &
                " UU_COY_ID = '" & pComp.CoyId & "' AND (" &
                " UU_APP_PKG NOT IN (" &
                " SELECT CA_APP_ID FROM COMPANY_APPLICATION" &
                " WHERE CA_COY_ID = UU_COY_ID) OR"

            If pComp.CoyType = "BOTH" Then
                strSQL &= " UGM_TYPE NOT IN ('BUYER', 'VENDOR')"
            Else
                strSQL &= " UGM_TYPE NOT IN ('" & pComp.CoyType & "')"
            End If
            strSQL &= "))"

            Common.Insert2Ary(Query, strSQL)

            If Query(0) <> String.Empty Then
                If objDb.BatchExecute(Query) Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Function
        Public Function getAllCompanyTypeAhead(Optional ByVal isRandom As String = "n") As DataSet 'Craven (14/03/2011) 
            Dim ds As DataSet

            Dim strSQL As String
            If isRandom = "y" Then
                strSQL = "SELECT CM_COY_ID, CM_COY_NAME FROM COMPANY_MSTR WHERE CM_DELETED = ""N"""
            Else
                strSQL = "SELECT CM_COY_ID, CM_COY_NAME FROM COMPANY_MSTR "

            End If


            ds = objDb.FillDs(strSQL)
            ds.Tables(0).TableName = "CompanyListName"


            Return ds

        End Function

        Public Function GetIPPCompanyDetails(ByVal pCompId As String, ByVal strCoyType As String, Optional ByVal strCoyIdOrName As String = "", Optional ByVal CompIdx As Integer = 0) As Company
            Dim strGet As String
            Dim dtCoy As DataTable
            Dim objCoyDetails As New Company

            If strCoyType = "V" And strCoyIdOrName <> "" Then 'when populating screen after modification
                strGet = "SELECT * FROM ipp_company WHERE IC_COY_ID = '" & Common.Parse(pCompId) & "' " _
                            & "AND IC_COY_NAME='" & Common.Parse(strCoyIdOrName) & "' and IC_COY_TYPE = '" & strCoyType & "' "
            ElseIf strCoyType = "B" And strCoyIdOrName <> "" Then
                strGet = "SELECT * FROM ipp_company WHERE IC_COY_ID = '" & Common.Parse(pCompId) & "' " _
                            & "AND IC_OTHER_B_COY_CODE='" & Common.Parse(strCoyIdOrName) & "' and IC_COY_TYPE = '" & strCoyType & "'"
            ElseIf strCoyType = "E" And strCoyIdOrName <> "" Then
                strGet = "SELECT * FROM ipp_company WHERE IC_COY_ID = '" & Common.Parse(pCompId) & "' and IC_COY_TYPE = '" & strCoyType & "' " _
                            & "AND IC_INDEX='" & Common.Parse(CompIdx) & "'"
            Else 'when populating screen
                strGet = "SELECT * FROM ipp_company WHERE IC_COY_ID = '" & Common.Parse(pCompId) & "'"
            End If

            If strGet = "NULL" Then
                dtCoy = Nothing
            Else
                dtCoy = objDb.FillDt(strGet)
            End If

            'Zulham 14072018 - PAMB
            'Workaround for vendor coming from VendorRegistration. 
            'Need prper codes
            If dtCoy.Rows.Count = 0 Then
                If Not CompIdx = 0 Then
                    strGet = "SELECT * FROM ipp_company WHERE IC_INDEX = '" & CompIdx & "' "
                    dtCoy = objDb.FillDt(strGet)
                End If
            End If

            If Not dtCoy Is Nothing Then
                With dtCoy.Rows(0)
                    objCoyDetails.CoyId = IIf(IsDBNull(.Item("IC_COY_ID")), "", .Item("IC_COY_ID"))
                    objCoyDetails.CoyType = IIf(IsDBNull(.Item("ic_coy_type")), "", .Item("ic_coy_type"))
                    objCoyDetails.BOtherCoy = IIf(IsDBNull(.Item("ic_other_b_coy_code")), "", .Item("ic_other_b_coy_code"))
                    objCoyDetails.BusinessRegNo = IIf(IsDBNull(.Item("ic_business_reg_no")), "", .Item("ic_business_reg_no"))
                    objCoyDetails.CoyName = IIf(IsDBNull(.Item("IC_COY_NAME")), "", .Item("IC_COY_NAME"))
                    objCoyDetails.Status = IIf(IsDBNull(.Item("ic_status")), "", .Item("ic_status"))
                    objCoyDetails.Address1 = IIf(IsDBNull(.Item("ic_addr_line1")), "", .Item("ic_addr_line1"))
                    objCoyDetails.Address2 = IIf(IsDBNull(.Item("ic_addr_line2")), "", .Item("ic_addr_line2"))
                    objCoyDetails.Address3 = IIf(IsDBNull(.Item("ic_addr_line3")), "", .Item("ic_addr_line3"))
                    objCoyDetails.City = IIf(IsDBNull(.Item("ic_city")), "", .Item("ic_city"))
                    objCoyDetails.State = IIf(IsDBNull(.Item("ic_state")), "", .Item("ic_state"))
                    objCoyDetails.PostCode = IIf(IsDBNull(.Item("ic_postcode")), "", .Item("ic_postcode"))
                    objCoyDetails.Country = IIf(IsDBNull(.Item("ic_country")), "", .Item("ic_country"))
                    objCoyDetails.ContactPerson = Common.parseNull(.Item("ic_contact"))
                    objCoyDetails.Phone = IIf(IsDBNull(.Item("ic_phone")), "", .Item("ic_phone"))
                    objCoyDetails.Fax = IIf(IsDBNull(.Item("ic_fax")), "", .Item("ic_fax"))
                    objCoyDetails.Email = IIf(IsDBNull(.Item("ic_email")), "", .Item("ic_email"))
                    objCoyDetails.WebSites = IIf(IsDBNull(.Item("ic_website")), "", .Item("ic_website"))
                    objCoyDetails.PaymentMethod = IIf(IsDBNull(.Item("ic_payment_method")), "", .Item("ic_payment_method"))
                    objCoyDetails.BankCode = IIf(IsDBNull(.Item("ic_bank_code")), "", .Item("ic_bank_code"))
                    objCoyDetails.AccountNo = IIf(IsDBNull(.Item("ic_bank_acct")), "", .Item("ic_bank_acct"))
                    objCoyDetails.BankName = IIf(IsDBNull(.Item("ic_bank_acct")), "", .Item("ic_bank_acct"))
                    objCoyDetails.BankAddrLine1 = IIf(IsDBNull(.Item("ic_bank_addr_line1")), "", .Item("ic_bank_addr_line1"))
                    objCoyDetails.BankAddrLine2 = IIf(IsDBNull(.Item("ic_bank_addr_line2")), "", .Item("ic_bank_addr_line2"))
                    objCoyDetails.BankAddrLine3 = IIf(IsDBNull(.Item("ic_bank_addr_line3")), "", .Item("ic_bank_addr_line3"))
                    objCoyDetails.BankCity = IIf(IsDBNull(.Item("ic_bank_city")), "", .Item("ic_bank_city"))
                    objCoyDetails.BankState = IIf(IsDBNull(.Item("ic_bank_state")), "", .Item("ic_bank_state"))
                    objCoyDetails.BankPostcode = IIf(IsDBNull(.Item("ic_bank_postcode")), "", .Item("ic_bank_postcode"))
                    objCoyDetails.BankCountry = IIf(IsDBNull(.Item("ic_bank_country")), "", .Item("ic_bank_country"))
                    objCoyDetails.ConIBSGLCode = IIf(IsDBNull(.Item("ic_con_ibs_code")), "", .Item("ic_con_ibs_code"))
                    objCoyDetails.NonConIBSGLCode = IIf(IsDBNull(.Item("ic_non_con_ibs_code")), "", .Item("ic_non_con_ibs_code"))
                    'objCoyDetails.RelatedCompany = IIf(IsDBNull(.Item("ic_related")), "", .Item("ic_related"))
                    objCoyDetails.WaiveCharges = IIf(IsDBNull(.Item("ic_waive_charges")), "", .Item("ic_waive_charges"))
                    objCoyDetails.CreditTerms = IIf(IsDBNull(.Item("ic_credit_terms")), "", .Item("ic_credit_terms"))
                    objCoyDetails.Currency = IIf(IsDBNull(.Item("ic_currency")), "", .Item("ic_currency"))
                    objCoyDetails.InactiveReason = IIf(IsDBNull(.Item("ic_remark")), "", .Item("ic_remark"))
                    objCoyDetails.CompanyCategory = IIf(IsDBNull(.Item("ic_company_category")), "", .Item("ic_company_category"))
                    objCoyDetails.ResidentType = IIf(IsDBNull(.Item("ic_resident_type")), "", .Item("ic_resident_type"))
                    objCoyDetails.ResidentCountry = IIf(IsDBNull(.Item("ic_resident_country")), "", .Item("ic_resident_country"))
                    'Chee Hong - IPP GST Enhancement - 9/9/2014
                    objCoyDetails.TaxRegNo = IIf(IsDBNull(.Item("IC_TAX_REG_NO")), "", .Item("IC_TAX_REG_NO"))
                    objCoyDetails.GSTDateLastStatus = IIf(IsDBNull(.Item("IC_LAST_DATE")), "", .Item("IC_LAST_DATE"))
                    objCoyDetails.GstInputTaxCode = IIf(IsDBNull(.Item("IC_GST_INPUT_TAX_CODE")), "", .Item("IC_GST_INPUT_TAX_CODE"))
                    objCoyDetails.GstOutputTaxCode = IIf(IsDBNull(.Item("IC_GST_OUTPUT_TAX_CODE")), "", .Item("IC_GST_OUTPUT_TAX_CODE"))
                    objCoyDetails.JobGrade = IIf(IsDBNull(.Item("IC_ADDITIONAL_1")), "", .Item("IC_ADDITIONAL_1"))
                    objCoyDetails.StaffCessationEffectiveDate = IIf(IsDBNull(.Item("IC_ADDITIONAL_2")), "", .Item("IC_ADDITIONAL_2"))
                    objCoyDetails.BranchCode = IIf(IsDBNull(.Item("IC_ADDITIONAL_3")), "", .Item("IC_ADDITIONAL_3"))
                    objCoyDetails.CostCentre = IIf(IsDBNull(.Item("IC_ADDITIONAL_4")), "", .Item("IC_ADDITIONAL_4"))
                    'Zulham 16/02/2015 8317
                    objCoyDetails.NostroIncome = IIf(IsDBNull(.Item("IC_nostro_flag")), "", .Item("IC_nostro_flag"))
                    '------------------------------------------

                    'Chee Hong - Issue 8317 - 26 Feb 2015
                    objCoyDetails.GstRegDate = IIf(IsDBNull(.Item("IC_GST_EFF_DATE")), "", .Item("IC_GST_EFF_DATE"))
                    objCoyDetails.NostroIncome = IIf(IsDBNull(.Item("IC_NOSTRO_FLAG")), "", .Item("IC_NOSTRO_FLAG"))
                    objCoyDetails.BillGLCode = IIf(IsDBNull(.Item("IC_BILL_GL_CODE")), "", .Item("IC_BILL_GL_CODE"))

                    objCoyDetails.SysValDate = IIf(IsDBNull(.Item("IC_SYSTEM_VALID_DATE")), "", .Item("IC_SYSTEM_VALID_DATE")) 'Jules 2015.08.14 - IPP Stage 4 Phase 2
                    '------------------------------------------
                End With
                GetIPPCompanyDetails = objCoyDetails
            Else
                GetIPPCompanyDetails = Nothing
            End If

        End Function

        'Public Function AddIPPCompany(ByVal pComp As Company, Optional ByVal strApp() As String = Nothing) As Boolean
        '    AddIPPCompany = False

        '    Dim strSql, strSqlExist, strSqlDel As String

        '    If (Common.Parse(pComp.CoyType)) = "V" Then  'Vendor/Employee
        '        'strSqlExist = "SELECT * FROM ipp_company WHERE IC_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' "
        '        strSqlExist = "SELECT * FROM ipp_company WHERE IC_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
        '                    & "AND IC_COY_NAME='" & Common.Parse(pComp.CoyName) & "' and ic_coy_type = 'V'"
        '    ElseIf (Common.Parse(pComp.CoyType)) = "E" Then
        '        strSqlExist = "SELECT * FROM ipp_company WHERE IC_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
        '                       & "and ic_business_reg_no = '" & Common.Parse(pComp.BusinessRegNo) & "'  and ic_coy_type = 'E'"
        '        '& "AND IC_COY_NAME='" & Common.Parse(pComp.CoyName) & "' and ic_business_reg_no = '" & Common.Parse(pComp.BusinessRegNo) & "'  and ic_coy_type = 'E'"

        '    Else
        '        strSqlExist = "SELECT * FROM ipp_company WHERE IC_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
        '                    & "AND IC_OTHER_B_COY_CODE='" & Common.Parse(pComp.BOtherCoy) & "'  and ic_coy_type <> 'E' and ic_coy_type <> 'V'"
        '    End If

        '    Try
        '        If objDb.Exist(strSqlExist) Then
        '            strCompMassage = Common.RecordDuplicate
        '            Return False
        '            'ElseIf objDb.Exist(strSqlDel) Then
        '            '    strCompMassage = Common.RecordUsed
        '            '    Return False
        '        Else
        '            ' ai chu add on 05/12/2005
        '            ' SR U30012 - to include additional fields to allow hub admin enter the 
        '            ' no of SKU and no of transacrion at the company detail screen
        '            'Modified for IPP Gst Stage 2A
        '            If (Common.Parse(pComp.CoyType)) = "V" Then
        '                strSql = "INSERT INTO IPP_COMPANY (IC_COY_ID,IC_OTHER_B_COY_CODE,IC_COY_NAME,IC_COY_TYPE, " _
        '                                           & "IC_STATUS,IC_BUSINESS_REG_NO,IC_ADDR_LINE1,IC_ADDR_LINE2,IC_ADDR_LINE3, " _
        '                                           & "IC_POSTCODE,IC_CITY,IC_STATE,IC_COUNTRY,IC_CONTACT,IC_PHONE,IC_FAX,IC_EMAIL, " _
        '                                           & "IC_WEBSITE,IC_BANK_CODE,IC_BANK_ACCT,IC_BANK_ADDR_LINE1,IC_BANK_ADDR_LINE2, " _
        '                                           & "IC_BANK_ADDR_LINE3,IC_BANK_POSTCODE,IC_BANK_CITY,IC_BANK_STATE,IC_BANK_COUNTRY, " _
        '                                           & "IC_PAYMENT_METHOD,IC_CON_IBS_CODE,IC_NON_CON_IBS_CODE,IC_ENT_BY,IC_ENT_DATETIME,IC_WAIVE_CHARGES,IC_CURRENCY,IC_CREDIT_TERMS,IC_COMPANY_CATEGORY,IC_RESIDENT_TYPE,IC_RESIDENT_COUNTRY," _
        '                                           & "IC_TAX_REG_NO,IC_LAST_DATE,IC_GST_INPUT_TAX_CODE,IC_GST_OUTPUT_TAX_CODE,IC_GST_EFF_DATE,IC_NOSTRO_FLAG,IC_BILL_GL_CODE) " _
        '                                           & "VALUES('" & Common.Parse(pComp.CoyId) & "','" & Common.Parse(pComp.BOtherCoy) & "','" &
        '                                           Common.Parse(pComp.CoyName) & "','" & Common.Parse(pComp.CoyType) & "','" & Common.Parse(pComp.Status) & "','" &
        '                                           Common.Parse(pComp.BusinessRegNo) & "','" & Common.Parse(pComp.Address1) & "','" & Common.Parse(pComp.Address2) & "','" &
        '                                           Common.Parse(pComp.Address3) & "','" & Common.Parse(pComp.PostCode) & "','" &
        '                                           Common.Parse(pComp.City) & "','" & Common.Parse(pComp.State) & "','" & Common.Parse(pComp.Country) & "','" &
        '                                           Common.Parse(pComp.ContactPerson) & "','" & Common.Parse(pComp.Phone) & "','" & Common.Parse(pComp.Fax) & "','" &
        '                                           Common.Parse(pComp.Email) & "','" & Common.Parse(pComp.WebSites) & "','" & Common.Parse(pComp.BankCode) & "','" &
        '                                           Common.Parse(pComp.AccountNo) & "','" & Common.Parse(pComp.BankAddrLine1) & "','" & Common.Parse(pComp.BankAddrLine2) & "','" & Common.Parse(pComp.BankAddrLine3) & "','" &
        '                                           Common.Parse(pComp.BankPostcode) & "','" & Common.Parse(pComp.BankCity) & "','" & Common.Parse(pComp.BankState) & "','" &
        '                                           Common.Parse(pComp.BankCountry) & "','" &
        '                                           Common.Parse(pComp.PaymentMethod) & "','" & Common.Parse(pComp.ConIBSGLCode) & "','" & Common.Parse(pComp.NonConIBSGLCode) & "','" &
        '                                           ctx.Session("UserId") & "',NOW(),'" & Common.Parse(Common.parseNull(pComp.WaiveCharges)) & "',NULL,'" & Common.Parse(Common.parseNull(pComp.CreditTerms)) & "', '" & Common.parseNull(pComp.CompanyCategory) & "', '" & Common.parseNull(pComp.ResidentType) & "', '" & Common.parseNull(pComp.ResidentCountry) & "','" &
        '                                           Common.Parse(pComp.TaxRegNo) & "'," & IIf(pComp.GSTDateLastStatus = "", "NULL", Common.ConvertDate(pComp.GSTDateLastStatus)) & ",'" & Common.Parse(pComp.GstInputTaxCode) & "','" & Common.Parse(pComp.GstOutputTaxCode) & "', " &
        '                                           IIf(pComp.GstRegDate = "", "NULL", Common.ConvertDate(pComp.GstRegDate)) & ", '" & pComp.NostroIncome & "', '" & Common.Parse(pComp.BillGLCode) & "')" & "" 'Modified for IPP Gst Stage 2A
        '            ElseIf (Common.Parse(pComp.CoyType)) = "E" Then
        '                'Zulham 29062015 - HLB-IPP GST Stage 4(CR)
        '                'Added 2 more columns for input/output gst tax code
        '                strSql = "INSERT INTO IPP_COMPANY (IC_COY_ID,IC_OTHER_B_COY_CODE,IC_COY_NAME,IC_COY_TYPE, " _
        '                   & "IC_STATUS,IC_BUSINESS_REG_NO,IC_ADDR_LINE1,IC_ADDR_LINE2,IC_ADDR_LINE3, " _
        '                   & "IC_POSTCODE,IC_CITY,IC_STATE,IC_COUNTRY,IC_CONTACT,IC_PHONE,IC_FAX,IC_EMAIL, " _
        '                   & "IC_WEBSITE,IC_BANK_CODE,IC_BANK_ACCT,IC_BANK_ADDR_LINE1,IC_BANK_ADDR_LINE2, " _
        '                   & "IC_BANK_ADDR_LINE3,IC_BANK_POSTCODE,IC_BANK_CITY,IC_BANK_STATE,IC_BANK_COUNTRY, " _
        '                   & "IC_PAYMENT_METHOD,IC_CON_IBS_CODE,IC_NON_CON_IBS_CODE,IC_ENT_BY,IC_ENT_DATETIME,IC_WAIVE_CHARGES,IC_ADDITIONAL_1,IC_ADDITIONAL_2,IC_ADDITIONAL_3,IC_ADDITIONAL_4,IC_GST_INPUT_TAX_CODE,IC_GST_OUTPUT_TAX_CODE) " _
        '                   & "VALUES('" & Common.Parse(pComp.CoyId) & "','" & Common.Parse(pComp.BOtherCoy) & "','" &
        '                   Common.Parse(pComp.CoyName) & "','" & Common.Parse(pComp.CoyType) & "','" & Common.Parse(pComp.Status) & "','" &
        '                   Common.Parse(pComp.BusinessRegNo) & "','" & Common.Parse(pComp.Address1) & "','" & Common.Parse(pComp.Address2) & "','" &
        '                   Common.Parse(pComp.Address3) & "','" & Common.Parse(pComp.PostCode) & "','" &
        '                   Common.Parse(pComp.City) & "','" & Common.Parse(pComp.State) & "','" & Common.Parse(pComp.Country) & "','" &
        '                   Common.Parse(pComp.ContactPerson) & "','" & Common.Parse(pComp.Phone) & "','" & Common.Parse(pComp.Fax) & "','" &
        '                   Common.Parse(pComp.Email) & "','" & Common.Parse(pComp.WebSites) & "','" & Common.Parse(pComp.BankCode) & "','" &
        '                   Common.Parse(pComp.AccountNo) & "','" & Common.Parse(pComp.BankAddrLine1) & "','" & Common.Parse(pComp.BankAddrLine2) & "','" & Common.Parse(pComp.BankAddrLine3) & "','" &
        '                   Common.Parse(pComp.BankPostcode) & "','" & Common.Parse(pComp.BankCity) & "','" & Common.Parse(pComp.BankState) & "','" &
        '                   Common.Parse(pComp.BankCountry) & "','" &
        '                   Common.Parse(pComp.PaymentMethod) & "','" & Common.Parse(pComp.ConIBSGLCode) & "','" & Common.Parse(pComp.NonConIBSGLCode) & "','" &
        '                   ctx.Session("UserId") & "',NOW(),NULL,'" & Common.Parse(pComp.JobGrade) & "'," & IIf(pComp.StaffCessationEffectiveDate = "", "NULL", Common.ConvertDate(pComp.StaffCessationEffectiveDate)) & ",'" & Common.Parse(pComp.BranchCode) & "','" & Common.Parse(pComp.CostCentre) & "','" & Common.Parse(Common.parseNull(pComp.GstInputTaxCode)) & "', '" & Common.parseNull(Common.Parse(pComp.GstOutputTaxCode)) & "')" & ""
        '            Else
        '                strSql = "INSERT INTO IPP_COMPANY (IC_COY_ID,IC_OTHER_B_COY_CODE,IC_COY_NAME,IC_COY_TYPE, " _
        '                   & "IC_STATUS,IC_BUSINESS_REG_NO,IC_ADDR_LINE1,IC_ADDR_LINE2,IC_ADDR_LINE3, " _
        '                   & "IC_POSTCODE,IC_CITY,IC_STATE,IC_COUNTRY,IC_CONTACT,IC_PHONE,IC_FAX,IC_EMAIL, " _
        '                   & "IC_WEBSITE,IC_BANK_CODE,IC_BANK_ACCT,IC_BANK_ADDR_LINE1,IC_BANK_ADDR_LINE2, " _
        '                   & "IC_BANK_ADDR_LINE3,IC_BANK_POSTCODE,IC_BANK_CITY,IC_BANK_STATE,IC_BANK_COUNTRY, " _
        '                   & "IC_PAYMENT_METHOD,IC_CON_IBS_CODE,IC_NON_CON_IBS_CODE,IC_ENT_BY,IC_ENT_DATETIME,IC_WAIVE_CHARGES,IC_CURRENCY) " _
        '                   & "VALUES('" & Common.Parse(pComp.CoyId) & "','" & Common.Parse(pComp.BOtherCoy) & "','" &
        '                   Common.Parse(pComp.CoyName) & "','" & Common.Parse(pComp.CoyType) & "','" & Common.Parse(pComp.Status) & "','" &
        '                   Common.Parse(pComp.BusinessRegNo) & "','" & Common.Parse(pComp.Address1) & "','" & Common.Parse(pComp.Address2) & "','" &
        '                   Common.Parse(pComp.Address3) & "','" & Common.Parse(pComp.PostCode) & "','" &
        '                   Common.Parse(pComp.City) & "','" & Common.Parse(pComp.State) & "','" & Common.Parse(pComp.Country) & "','" &
        '                   Common.Parse(pComp.ContactPerson) & "','" & Common.Parse(pComp.Phone) & "','" & Common.Parse(pComp.Fax) & "','" &
        '                   Common.Parse(pComp.Email) & "','" & Common.Parse(pComp.WebSites) & "','" & Common.Parse(pComp.BankCode) & "','" &
        '                   Common.Parse(pComp.AccountNo) & "','" & Common.Parse(pComp.BankAddrLine1) & "','" & Common.Parse(pComp.BankAddrLine2) & "','" & Common.Parse(pComp.BankAddrLine3) & "','" &
        '                   Common.Parse(pComp.BankPostcode) & "','" & Common.Parse(pComp.BankCity) & "','" & Common.Parse(pComp.BankState) & "','" &
        '                   Common.Parse(pComp.BankCountry) & "','" &
        '                   Common.Parse(pComp.PaymentMethod) & "','" & Common.Parse(pComp.ConIBSGLCode) & "','" & Common.Parse(pComp.NonConIBSGLCode) & "','" &
        '                   ctx.Session("UserId") & "',NOW(),NULL,'" & pComp.Currency & "' )" & ""
        '            End If


        '            If objDb.Execute(strSql) Then
        '                'AddCompParam(pComp)
        '                ''Michelle (21/1/2011) - To set the Invoice Approval Rule to True if there the company is with Invoice Approval
        '                '' AddCompSetting(pComp)
        '                'If Common.Parse(pComp.InvAppr) = "N" Then
        '                '    AddCompSetting(pComp)
        '                'Else
        '                '    AddCompSetting(pComp, "Y")
        '                'End If
        '                'AddExchnageRate(pComp)

        '                'If Common.Parse(pComp.CoyType) = "BOTH" Or Common.Parse(pComp.CoyType) = "BUYER" Then AddBCompDefault(Common.Parse(pComp.CoyId))

        '                'If Not strApp Is Nothing Then
        '                '    addAppPackage(pComp, strApp)
        '                'End If

        '                Message = Common.RecordSave
        '                Return True
        '            End If
        '        End If
        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        Public Function check_ipp_coyname(ByVal strCoyName As String, Optional ByVal strRegNo As String = "", Optional ByVal strCompType As String = "") As Boolean
            Dim strSql As String

            If strCompType = "E" Then
                strSql = "SELECT * FROM ipp_company WHERE ic_business_reg_no = '" & strRegNo & "' and ic_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  and ic_coy_type = 'E' "
                'strSql = "SELECT * FROM ipp_company WHERE IC_COY_NAME='" & Common.Parse(strCoyName) & "' and ic_business_reg_no = '" & strRegNo & "' and ic_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  and ic_coy_type = 'E' "
            ElseIf strCompType = "V" Then
                strSql = "SELECT * FROM ipp_company WHERE IC_COY_NAME='" & Common.Parse(strCoyName) & "' and ic_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' and ic_coy_type = 'V'  "
            Else
                strSql = "SELECT * FROM ipp_company WHERE IC_COY_NAME='" & Common.Parse(strCoyName) & "' and ic_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' and ic_coy_type <> 'E' and ic_coy_type <> 'V' "
            End If

            If objDb.Exist(strSql) Then
                strCompMassage = Common.RecordDuplicate
                Return False
            Else
                Return True
            End If
        End Function

        'Public Function getGSTRegNo(Optional ByVal strCoyId As String = "") As String
        '    Dim strSQL As String
        '    Dim strGstCOD As String
        '    strGstCOD = ConfigurationManager.AppSettings.Get("GstCutOffDate")

        '    If strCoyId = "" Then
        '        strCoyId = HttpContext.Current.Session("CompanyID")
        '    End If

        '    If Date.Now() >= CDate(strGstCOD) Then
        '        strSQL = "SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyId & "'"
        '        getGSTRegNo = objDb.GetVal(strSQL)
        '    Else
        '        getGSTRegNo = ""
        '    End If

        'End Function



#Region " from eRFP"

        'Public Function AddTempCompany(ByVal pComp As Company, Optional ByVal strApp() As String = Nothing) As Boolean
        '    AddTempCompany = False

        '    Dim strSql, strSqlExist, strSqlDel, strSqlExistTmp, strSqlDelTmp As String

        '    strSqlExist = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
        '          Common.Parse(pComp.CoyId) & "' AND CM_DELETED <>'Y'"

        '    strSqlDel = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
        '           Common.Parse(pComp.CoyId) & "' AND CM_DELETED='Y'"

        '    strSqlExistTmp = "SELECT * FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID='" &
        '          Common.Parse(pComp.CoyId) & "' AND CM_DELETED <>'Y'"

        '    strSqlDelTmp = "SELECT * FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID='" &
        '           Common.Parse(pComp.CoyId) & "' AND CM_DELETED='Y'"

        '    Try

        '        If objDb.Exist(strSqlExist) Then
        '            strCompMassage = Common.RecordDuplicate
        '            Return False
        '        ElseIf objDb.Exist(strSqlDel) Then
        '            strCompMassage = Common.RecordUsed
        '            Return False
        '        ElseIf objDb.Exist(strSqlExistTmp) Then
        '            strCompMassage = Common.RecordDuplicate
        '            Return False
        '        ElseIf objDb.Exist(strSqlDelTmp) Then
        '            strCompMassage = Common.RecordUsed
        '            Return False
        '        Else

        '            'strSql = "INSERT INTO COMPANY_MSTR_TEMP(CM_COY_ID,CM_COY_NAME,CM_STATUS," & _
        '            '        "CM_COY_TYPE,CM_LICENCE_PACKAGE, " & _
        '            '        "CM_LICENSE_USERS,CM_SUB_START_DT,CM_SUB_END_DT," & _
        '            '        "CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3," & _
        '            '        "CM_CITY,CM_STATE,CM_POSTCODE," & _
        '            '        "CM_COUNTRY,CM_PHONE,CM_FAX," & _
        '            '        "CM_EMAIL,CM_BUSINESS_REG_NO,CM_ACCT_NO," & _
        '            '        "CM_BANK,CM_BRANCH,CM_CURRENCY_CODE," & _
        '            '        "CM_COY_LOGO,CM_ACTUAL_TERMSANDCONDFILE,CM_HUB_TERMSANDCONDFILE,CM_TAX_REG_NO," & _
        '            '        "CM_TAX_CALC_BY,CM_BCM_SET,CM_FINDEPT_MODE," & _
        '            '        "CM_PAYMENT_TERM,CM_PAYMENT_METHOD,CM_PWD_DURATION," & _
        '            '        "CM_SKINS_ID,CM_PRIV_LABELING,CM_ENT_BY,CM_ENT_DT) " & _
        '            '"VALUES('" & Common.Parse(pComp.CoyId) & "','" & Common.Parse(pComp.CoyName) & "','" & Common.Parse(pComp.Status) & "','" & _
        '            '        Common.Parse(pComp.CoyType) & "','" & Common.Parse(pComp.Package) & "'," & _
        '            '        Common.Parse(pComp.LicenseUsers) & "," & Common.ConvertDate(pComp.SubStart) & "," & Common.ConvertDate(pComp.SubEnd) & ",'" & _
        '            '        Common.Parse(pComp.Address1) & "','" & Common.Parse(pComp.Address2) & "','" & Common.Parse(pComp.Address3) & "','" & _
        '            '        Common.Parse(pComp.City) & "','" & Common.Parse(pComp.State) & "','" & Common.Parse(pComp.PostCode) & "','" & _
        '            '        Common.Parse(pComp.Country) & "','" & Common.Parse(pComp.Phone) & "','" & Common.Parse(pComp.Fax) & "','" & _
        '            '        Common.Parse(pComp.Email) & "','" & Common.Parse(pComp.BusinessRegNo) & "','" & Common.Parse(pComp.AccountNo) & "','" & _
        '            '        Common.Parse(pComp.BankCode) & "','" & Common.Parse(pComp.BranchCode) & "','" & Common.Parse(pComp.Currency) & "','" & _
        '            '        Common.Parse(pComp.CoyLogo) & "','" & Common.Parse(pComp.Actual_TC) & "','" & Common.Parse(pComp.TC) & "','" & Common.Parse(pComp.TaxRegNo) & "','" & _
        '            '        Common.Parse(pComp.TaxCalBy) & "','" & Common.Parse(pComp.BCMSetting) & "','" & Common.Parse(pComp.FinDeptMode) & "','" & _
        '            '        Common.Parse(pComp.PaymentTerm) & "','" & Common.Parse(pComp.PaymentMethod) & "','" & Common.Parse(pComp.PwdDuration) & "','" & _
        '            '        Common.Parse(pComp.Skins) & "','" & Common.Parse(pComp.PrivLabeling) & "','" & ctx.Session("UserId") & "',GETDATE())"

        '            strSql = "INSERT INTO COMPANY_MSTR_TEMP(CM_COY_ID,CM_COY_NAME,CM_COY_TYPE," &
        '                    "CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3," &
        '                    "CM_CITY,CM_STATE,CM_POSTCODE," &
        '                    "CM_COUNTRY,CM_PHONE,CM_FAX," &
        '                    "CM_EMAIL,CM_BUSINESS_REG_NO,CM_ACCT_NO," &
        '                    "CM_BANK,CM_BRANCH,CM_CURRENCY_CODE," &
        '                    "CM_COY_LOGO,CM_ACTUAL_TERMSANDCONDFILE,CM_HUB_TERMSANDCONDFILE,CM_TAX_REG_NO," &
        '                    "CM_TAX_CALC_BY,CM_SKINS_ID," &
        '                    "CM_ENT_BY,CM_ENT_DT,CM_REG_DATE) " &
        '            "VALUES('" & Common.Parse(pComp.CoyId) & "','" & Common.Parse(pComp.CoyName) & "','" & Common.Parse(pComp.CoyType) & "','" &
        '                    Common.Parse(pComp.Address1) & "','" & Common.Parse(pComp.Address2) & "','" & Common.Parse(pComp.Address3) & "','" &
        '                    Common.Parse(pComp.City) & "','" & Common.Parse(pComp.State) & "','" & Common.Parse(pComp.PostCode) & "','" &
        '                    Common.Parse(pComp.Country) & "','" & Common.Parse(pComp.Phone) & "','" & Common.Parse(pComp.Fax) & "','" &
        '                    Common.Parse(pComp.Email) & "','" & Common.Parse(pComp.BusinessRegNo) & "','" & Common.Parse(pComp.AccountNo) & "','" &
        '                    Common.Parse(pComp.BankCode) & "','" & Common.Parse(pComp.BranchCode) & "','" & Common.Parse(pComp.Currency) & "','" &
        '                    Common.Parse(pComp.CoyLogo) & "','" & Common.Parse(pComp.Actual_TC) & "','" & Common.Parse(pComp.TC) & "','" & Common.Parse(pComp.TaxRegNo) & "','" &
        '                    0 & "','1','"
        '            '0 & "','1','" & _
        '            'ctx.Session("UserId") & "',GETDATE(),GETDATE())"
        '            If ctx.Session("UserId") <> "" Then
        '                strSql &= ctx.Session("UserId")
        '            End If
        '            strSql &= "',GETDATE(),GETDATE())"

        '            If objDb.Execute(strSql) Then
        '                Message = Common.RecordSave
        '                Return True
        '            End If
        '        End If
        '    Catch errExp As CustomException
        '        Message = errExp.Message
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Message = errExp1.Message
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        'Function UpdateCompany(ByVal pComp As Company, ByVal strFlagValue As String, Optional ByVal blnSetting As Boolean = False) As Boolean

        '    Dim strSql As String

        '    strSql = "UPDATE COMPANY_MSTR SET CM_COY_NAME='" & Common.Parse(pComp.CoyName) & "'," &
        '    "CM_STATUS='" & Common.Parse(pComp.Status) & "'," &
        '    "CM_COY_TYPE='" & Common.Parse(pComp.CoyType) & "'," &
        '    "CM_PARENT_COY_ID='" & Common.Parse(pComp.ParentCoy) & "'," &
        '    "CM_LICENCE_PACKAGE='" & Common.Parse(pComp.Package) & "', " &
        '    "CM_LICENSE_USERS='" & Common.Parse(pComp.LicenseUsers) & "', " &
        '    "CM_SUB_START_DT=" & Common.ConvertDate(pComp.SubStart) & ", " &
        '    "CM_SUB_END_DT=" & Common.ConvertDate(pComp.SubEnd) & ", " &
        '    "CM_ADDR_LINE1='" & Common.Parse(pComp.Address1) & "'," &
        '    "CM_ADDR_LINE2 ='" & Common.Parse(pComp.Address2) & "'," &
        '    "CM_ADDR_LINE3 ='" & Common.Parse(pComp.Address3) & "'," &
        '    "CM_CITY ='" & Common.Parse(pComp.City) & "'," &
        '    "CM_STATE='" & Common.Parse(pComp.State) & "'," &
        '    "CM_POSTCODE ='" & Common.Parse(pComp.PostCode) & "'," &
        '    "CM_COUNTRY ='" & Common.Parse(pComp.Country) & "'," &
        '    "CM_PHONE='" & Common.Parse(pComp.Phone) & "'," &
        '    "CM_FAX ='" & Common.Parse(pComp.Fax) & "'," &
        '    "CM_EMAIL ='" & Common.Parse(pComp.Email) & "'," &
        '    "CM_BUSINESS_REG_NO ='" & Common.Parse(pComp.BusinessRegNo) & "'," &
        '    "CM_ACCT_NO ='" & Common.Parse(pComp.AccountNo) & "'," &
        '    "CM_BANK='" & Common.Parse(pComp.BankCode) & "'," &
        '    "CM_BRANCH='" & Common.Parse(pComp.BranchCode) & "'," &
        '    "CM_CURRENCY_CODE='" & Common.Parse(pComp.Currency) & "', " &
        '    "CM_COY_LOGO='" & Common.Parse(pComp.CoyLogo) & "'," &
        '    "CM_HUB_TERMSANDCONDFILE='" & Common.Parse(pComp.TC) & "'," &
        '    "CM_ACTUAL_TERMSANDCONDFILE='" & Common.Parse(pComp.Actual_TC) & "'," &
        '    "CM_TAX_REG_NO ='" & Common.Parse(pComp.TaxRegNo) & "'," &
        '    "CM_TAX_CALC_BY='" & Common.Parse(pComp.TaxCalBy) & "'," &
        '    "CM_BCM_SET='" & Common.Parse(pComp.BCMSetting) & "'," &
        '    "CM_FINDEPT_MODE='" & Common.Parse(pComp.FinDeptMode) & "'," &
        '    "CM_INV_APPR='" & Common.Parse(pComp.InvAppr) & "'," &
        '    "CM_PAYMENT_TERM ='" & Common.Parse(pComp.PaymentTerm) & "'," &
        '    "CM_PAYMENT_METHOD ='" & Common.Parse(pComp.PaymentMethod) & "'," &
        '    "CM_PWD_DURATION=" & Common.Parse(pComp.PwdDuration) & "," &
        '    "CM_PRIV_LABELING='" & Common.Parse(pComp.PrivLabeling) & "'," &
        '    "CM_CONTACT='" & Common.Parse(pComp.ContactPerson) & "'," &
        '    "CM_REPORT_USERS=" & Common.Parse(pComp.ReportUsers) & "," &
        '    "CM_MOD_BY='" & ctx.Session("UserId") & "'," &
        '    "CM_MULTI_PO='" & Common.Parse(pComp.MultiInvAppr) & "'," &
        '    "CM_BA_CANCEL='" & Common.Parse(pComp.BACanPO) & "'," &
        '    "CM_MOD_DT=GETDATE()"
        '    '"WHERE CM_COY_ID ='" & Common.Parse(pComp.CoyId) & "'"

        '    ' ai chu add on 05/12/2005
        '    ' SR U30012 - to include additional fields to allow hub admin enter the 
        '    ' no of SKU and no of transacrion at the company detail screen
        '    If pComp.CoyType = "VENDOR" Or pComp.CoyType = "BOTH" Then
        '        'strSql &= ", CM_SKU = " & pComp.SKU & ", CM_TRANS_NO = " & pComp.TransNo & " "
        '        strSql &= ", CM_SKU = " & IIf(Common.Parse(pComp.SKU) = "", "NULL", Common.Parse(pComp.SKU)) & ", CM_TRANS_NO = " & IIf(Common.Parse(pComp.TransNo) = "", "NULL", Common.Parse(pComp.TransNo)) & " "
        '    End If

        '    strSql &= "WHERE CM_COY_ID ='" & Common.Parse(pComp.CoyId) & "'"


        '    If objDb.Execute(strSql) Then
        '        If blnSetting Then
        '            AddCompSetting(strFlagValue)
        '            Message = Common.RecordSave
        '        End If
        '        UpdateCompany = WheelMsgNum.Save
        '    Else
        '        UpdateCompany = WheelMsgNum.NotSave
        '    End If
        'End Function

        'Function UpdateBCompany(ByVal pComp As Company, ByVal strFlagValue As String, Optional ByVal blnSetting As Boolean = False) As Integer

        '    Dim strSql As String
        '    'Michelle (2/9/2010) - To cater for eRFP
        '    'strSql = "UPDATE COMPANY_MSTR SET CM_COY_NAME='" & Common.Parse(pComp.CoyName) & "'," & _
        '    '"CM_COY_TYPE='" & Common.Parse(pComp.CoyType) & "'," & _
        '    '"CM_PARENT_COY_ID='" & Common.Parse(pComp.ParentCoy) & "'," & _
        '    '"CM_LICENCE_PACKAGE='" & Common.Parse(pComp.Package) & "', " & _
        '    '"CM_LICENSE_USERS='" & Common.Parse(pComp.LicenseUsers) & "', " & _
        '    '"CM_SUB_START_DT=" & Common.ConvertDate(pComp.SubStart) & ", " & _
        '    '"CM_SUB_END_DT=" & Common.ConvertDate(pComp.SubEnd) & ", " & _
        '    '"CM_ADDR_LINE1='" & Common.Parse(pComp.Address1) & "'," & _
        '    '"CM_ADDR_LINE2 ='" & Common.Parse(pComp.Address2) & "'," & _
        '    '"CM_ADDR_LINE3 ='" & Common.Parse(pComp.Address3) & "'," & _
        '    '"CM_CITY ='" & Common.Parse(pComp.City) & "'," & _
        '    '"CM_STATE='" & Common.Parse(pComp.State) & "'," & _
        '    '"CM_POSTCODE ='" & Common.Parse(pComp.PostCode) & "'," & _
        '    '"CM_COUNTRY ='" & Common.Parse(pComp.Country) & "'," & _
        '    '"CM_PHONE='" & Common.Parse(pComp.Phone) & "'," & _
        '    '"CM_FAX ='" & Common.Parse(pComp.Fax) & "'," & _
        '    '"CM_EMAIL ='" & Common.Parse(pComp.Email) & "'," & _
        '    '"CM_BUSINESS_REG_NO ='" & Common.Parse(pComp.BusinessRegNo) & "'," & _
        '    '"CM_ACCT_NO ='" & Common.Parse(pComp.AccountNo) & "'," & _
        '    '"CM_BANK='" & Common.Parse(pComp.BankCode) & "'," & _
        '    '"CM_BRANCH='" & Common.Parse(pComp.BranchCode) & "'," & _
        '    '"CM_CURRENCY_CODE='" & Common.Parse(pComp.Currency) & "', " & _
        '    '"CM_COY_LOGO='" & Common.Parse(pComp.CoyLogo) & "'," & _
        '    '"CM_HUB_TERMSANDCONDFILE='" & Common.Parse(pComp.TC) & "'," & _
        '    '"CM_ACTUAL_TERMSANDCONDFILE='" & Common.Parse(pComp.Actual_TC) & "'," & _
        '    '"CM_TAX_REG_NO ='" & Common.Parse(pComp.TaxRegNo) & "'," & _
        '    '"CM_TAX_CALC_BY='" & Common.Parse(pComp.TaxCalBy) & "'," & _
        '    '"CM_BCM_SET='" & Common.Parse(pComp.BCMSetting) & "'," & _
        '    '"CM_FINDEPT_MODE='" & Common.Parse(pComp.FinDeptMode) & "'," & _
        '    '"CM_INV_APPR='" & Common.Parse(pComp.InvAppr) & "'," & _
        '    '"CM_PAYMENT_TERM ='" & Common.Parse(pComp.PaymentTerm) & "'," & _
        '    '"CM_PAYMENT_METHOD ='" & Common.Parse(pComp.PaymentMethod) & "'," & _
        '    '"CM_PWD_DURATION=" & Common.Parse(pComp.PwdDuration) & "," & _
        '    '"CM_PRIV_LABELING='" & Common.Parse(pComp.PrivLabeling) & "'," & _
        '    '"CM_CONTACT='" & Common.Parse(pComp.ContactPerson) & "'," & _
        '    '"CM_REPORT_USERS=" & Common.Parse(pComp.ReportUsers) & "," & _
        '    '"CM_MOD_BY='" & ctx.Session("UserId") & "'," & _
        '    '"CM_MULTI_PO='" & Common.Parse(pComp.MultiInvAppr) & "'," & _
        '    '"CM_BA_CANCEL='" & Common.Parse(pComp.BACanPO) & "'," & _
        '    '"CM_MOD_DT=GETDATE()" & _
        '    '"WHERE CM_COY_ID ='" & Common.Parse(pComp.CoyId) & "'"

        '    strSql = "UPDATE COMPANY_MSTR SET CM_COY_NAME='" & Common.Parse(pComp.CoyName) & "'," &
        '    "CM_COY_TYPE='" & Common.Parse(pComp.CoyType) & "'," &
        '    "CM_PARENT_COY_ID='" & Common.Parse(pComp.ParentCoy) & "'," &
        '    "CM_LICENCE_PACKAGE='" & Common.Parse(pComp.Package) & "', " &
        '    "CM_LICENSE_USERS='" & Common.Parse(pComp.LicenseUsers) & "', " &
        '    "CM_SUB_START_DT=" & Common.ConvertDate(pComp.SubStart) & ", " &
        '    "CM_SUB_END_DT=" & Common.ConvertDate(pComp.SubEnd) & ", " &
        '    "CM_ADDR_LINE1='" & Common.Parse(pComp.Address1) & "'," &
        '    "CM_ADDR_LINE2 ='" & Common.Parse(pComp.Address2) & "'," &
        '    "CM_ADDR_LINE3 ='" & Common.Parse(pComp.Address3) & "'," &
        '    "CM_CITY ='" & Common.Parse(pComp.City) & "'," &
        '    "CM_STATE='" & Common.Parse(pComp.State) & "'," &
        '    "CM_POSTCODE ='" & Common.Parse(pComp.PostCode) & "'," &
        '    "CM_COUNTRY ='" & Common.Parse(pComp.Country) & "'," &
        '    "CM_PHONE='" & Common.Parse(pComp.Phone) & "'," &
        '    "CM_FAX ='" & Common.Parse(pComp.Fax) & "'," &
        '    "CM_EMAIL ='" & Common.Parse(pComp.Email) & "'," &
        '    "CM_BUSINESS_REG_NO ='" & Common.Parse(pComp.BusinessRegNo) & "'," &
        '    "CM_ACCT_NO ='" & Common.Parse(pComp.AccountNo) & "'," &
        '    "CM_BANK='" & Common.Parse(pComp.BankCode) & "'," &
        '    "CM_BRANCH='" & Common.Parse(pComp.BranchCode) & "'," &
        '    "CM_CURRENCY_CODE='" & Common.Parse(pComp.Currency) & "', " &
        '    "CM_COY_LOGO='" & Common.Parse(pComp.CoyLogo) & "'," &
        '    "CM_HUB_TERMSANDCONDFILE='" & Common.Parse(pComp.TC) & "'," &
        '    "CM_ACTUAL_TERMSANDCONDFILE='" & Common.Parse(pComp.Actual_TC) & "'," &
        '    "CM_TAX_REG_NO ='" & Common.Parse(pComp.TaxRegNo) & "'," &
        '    "CM_TAX_CALC_BY='" & Common.Parse(pComp.TaxCalBy) & "'," &
        '    "CM_BCM_SET='" & Common.Parse(pComp.BCMSetting) & "'," &
        '    "CM_FINDEPT_MODE='" & Common.Parse(pComp.FinDeptMode) & "'," &
        '    "CM_PAYMENT_TERM ='" & Common.Parse(pComp.PaymentTerm) & "'," &
        '    "CM_PAYMENT_METHOD ='" & Common.Parse(pComp.PaymentMethod) & "'," &
        '    "CM_PWD_DURATION=" & Common.Parse(pComp.PwdDuration) & "," &
        '    "CM_PRIV_LABELING='" & Common.Parse(pComp.PrivLabeling) & "'," &
        '    "CM_CONTACT='" & Common.Parse(pComp.ContactPerson) & "'," &
        '    "CM_MOD_BY='" & ctx.Session("UserId") & "'," &
        '    "CM_MOD_DT=GETDATE()" &
        '    "WHERE CM_COY_ID ='" & Common.Parse(pComp.CoyId) & "'"

        '    If objDb.Execute(strSql) Then
        '        If blnSetting Then
        '            AddCompSetting(strFlagValue)
        '            Message = Common.RecordSave
        '        End If
        '        UpdateBCompany = WheelMsgNum.Save
        '    Else
        '        UpdateBCompany = WheelMsgNum.NotSave
        '    End If
        'End Function


        ''*************************************************************************************
        'Created By:  Ya Li
        'Date:  25/05/2005
        'Screen:  Public Vendor Registration Approval
        'Purpose:  Update COMPANY_MSTR_TEMP details.
        '**************************************************************************************
        'Function UpdateComDetails(ByVal pOriCoyId As String, ByVal pComp As Company) As Boolean

        '    'Dim strSql As String
        '    'strSql = "UPDATE COMPANY_MSTR_TEMP SET CM_COY_NAME='" & Common.Parse(pComp.CoyName) & "'," & _
        '    '"CM_STATUS='" & Common.Parse(pComp.Status) & "'," & _
        '    '"CM_ADDR_LINE1='" & Common.Parse(pComp.Address1) & "'," & _
        '    '"CM_ADDR_LINE2 ='" & Common.Parse(pComp.Address2) & "'," & _
        '    '"CM_ADDR_LINE3 ='" & Common.Parse(pComp.Address3) & "'," & _
        '    '"CM_CITY ='" & Common.Parse(pComp.City) & "'," & _
        '    '"CM_STATE='" & Common.Parse(pComp.State) & "'," & _
        '    '"CM_POSTCODE ='" & Common.Parse(pComp.PostCode) & "'," & _
        '    '"CM_COUNTRY ='" & Common.Parse(pComp.Country) & "'," & _
        '    '"CM_PHONE='" & Common.Parse(pComp.Phone) & "'," & _
        '    '"CM_FAX ='" & Common.Parse(pComp.Fax) & "'," & _
        '    '"CM_EMAIL ='" & Common.Parse(pComp.Email) & "'," & _
        '    '"CM_BUSINESS_REG_NO ='" & Common.Parse(pComp.BusinessRegNo) & "'," & _
        '    '"CM_ACCT_NO ='" & Common.Parse(pComp.AccountNo) & "'," & _
        '    '"CM_BANK='" & Common.Parse(pComp.BankCode) & "'," & _
        '    '"CM_BRANCH='" & Common.Parse(pComp.BranchCode) & "'," & _
        '    '"CM_CURRENCY_CODE='" & Common.Parse(pComp.Currency) & "', " & _
        '    '"CM_COY_LOGO='" & Common.Parse(pComp.CoyLogo) & "'," & _
        '    '"CM_HUB_TERMSANDCONDFILE='" & Common.Parse(pComp.TC) & "'," & _
        '    '"CM_ACTUAL_TERMSANDCONDFILE='" & Common.Parse(pComp.Actual_TC) & "'," & _
        '    '"CM_TAX_REG_NO ='" & Common.Parse(pComp.TaxRegNo) & "'," & _
        '    '"CM_TAX_CALC_BY='" & Common.Parse(pComp.TaxCalBy) & "'," & _
        '    '"CM_BCM_SET='" & Common.Parse(pComp.BCMSetting) & "'," & _
        '    '"CM_FINDEPT_MODE='" & Common.Parse(pComp.FinDeptMode) & "'," & _
        '    '"CM_PWD_DURATION='" & Common.Parse(pComp.PwdDuration) & "'," & _
        '    '"CM_PRIV_LABELING='" & Common.Parse(pComp.PrivLabeling) & "'," & _
        '    '"CM_SKINS_ID='" & Common.Parse(pComp.Skins) & "'," & _
        '    '"CM_TRAINING='" & Common.Parse(pComp.TrainDemo) & "'," & _
        '    '"CM_MOD_BY='" & ctx.Session("UserId") & "', " & _
        '    '"CM_MOD_DT=" & Common.ConvertDate(Now) & " " & _
        '    '"WHERE CM_COY_ID ='" & Common.Parse(pComp.CoyId) & "'"

        '    Dim strSql, strSqlExist, strSqlDel, strSqlExistTmp, strSqlDelTmp As String

        '    UpdateComDetails = False

        '    strSqlExist = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
        '          Common.Parse(pComp.CoyId) & "' AND CM_DELETED <>'Y'"

        '    strSqlDel = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
        '           Common.Parse(pComp.CoyId) & "' AND CM_DELETED='Y'"

        '    strSqlExistTmp = "SELECT * FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID='" &
        '          Common.Parse(pComp.CoyId) & "' AND CM_DELETED <>'Y'"

        '    strSqlDelTmp = "SELECT * FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID='" &
        '           Common.Parse(pComp.CoyId) & "' AND CM_DELETED='Y'"


        '    Try
        '        If pComp.CoyId <> pOriCoyId Then
        '            If objDb.Exist(strSqlExist) Then
        '                strCompMassage = Common.RecordDuplicate
        '                Return False
        '            ElseIf objDb.Exist(strSqlDel) Then
        '                strCompMassage = Common.RecordUsed
        '                Return False
        '            ElseIf objDb.Exist(strSqlExistTmp) Then
        '                strCompMassage = Common.RecordDuplicate
        '                Return False
        '            ElseIf objDb.Exist(strSqlDelTmp) Then
        '                strCompMassage = Common.RecordUsed
        '                Return False
        '            End If
        '        End If

        '        strSql = "UPDATE COMPANY_MSTR_TEMP SET " &
        '            "CM_COY_ID='" & Common.Parse(pComp.CoyId) & "'," &
        '            "CM_COY_NAME='" & Common.Parse(pComp.CoyName) & "'," &
        '            "CM_STATUS='" & Common.Parse(pComp.Status) & "'," &
        '            "CM_COY_TYPE='" & Common.Parse(pComp.CoyType) & "'," &
        '            "CM_PARENT_COY_ID='" & Common.Parse(pComp.ParentCoy) & "'," &
        '            "CM_LICENCE_PACKAGE='" & Common.Parse(pComp.Package) & "', " &
        '            "CM_LICENSE_USERS='" & Common.Parse(pComp.LicenseUsers) & "', " &
        '            "CM_SUB_START_DT=" & Common.ConvertDate(pComp.SubStart) & ", " &
        '            "CM_SUB_END_DT=" & Common.ConvertDate(pComp.SubEnd) & ", " &
        '            "CM_ADDR_LINE1='" & Common.Parse(pComp.Address1) & "'," &
        '            "CM_ADDR_LINE2 ='" & Common.Parse(pComp.Address2) & "'," &
        '            "CM_ADDR_LINE3 ='" & Common.Parse(pComp.Address3) & "'," &
        '            "CM_CITY ='" & Common.Parse(pComp.City) & "'," &
        '            "CM_STATE='" & Common.Parse(pComp.State) & "'," &
        '            "CM_POSTCODE ='" & Common.Parse(pComp.PostCode) & "'," &
        '            "CM_COUNTRY ='" & Common.Parse(pComp.Country) & "'," &
        '            "CM_PHONE='" & Common.Parse(pComp.Phone) & "'," &
        '            "CM_FAX ='" & Common.Parse(pComp.Fax) & "'," &
        '            "CM_EMAIL ='" & Common.Parse(pComp.Email) & "'," &
        '            "CM_BUSINESS_REG_NO ='" & Common.Parse(pComp.BusinessRegNo) & "'," &
        '            "CM_ACCT_NO ='" & Common.Parse(pComp.AccountNo) & "'," &
        '            "CM_BANK='" & Common.Parse(pComp.BankCode) & "'," &
        '            "CM_BRANCH='" & Common.Parse(pComp.BranchCode) & "'," &
        '            "CM_CURRENCY_CODE='" & Common.Parse(pComp.Currency) & "', " &
        '            "CM_COY_LOGO='" & Common.Parse(pComp.CoyLogo) & "'," &
        '            "CM_HUB_TERMSANDCONDFILE='" & Common.Parse(pComp.TC) & "'," &
        '            "CM_ACTUAL_TERMSANDCONDFILE='" & Common.Parse(pComp.Actual_TC) & "'," &
        '            "CM_TAX_REG_NO ='" & Common.Parse(pComp.TaxRegNo) & "'," &
        '            "CM_TAX_CALC_BY='" & Common.Parse(pComp.TaxCalBy) & "'," &
        '            "CM_BCM_SET='" & Common.Parse(pComp.BCMSetting) & "'," &
        '            "CM_FINDEPT_MODE='" & Common.Parse(pComp.FinDeptMode) & "'," &
        '            "CM_INV_APPR='" & Common.Parse(pComp.InvAppr) & "'," &
        '            "CM_PAYMENT_TERM ='" & Common.Parse(pComp.PaymentTerm) & "'," &
        '            "CM_PAYMENT_METHOD ='" & Common.Parse(pComp.PaymentMethod) & "'," &
        '            "CM_PWD_DURATION='" & Common.Parse(pComp.PwdDuration) & "'," &
        '            "CM_PRIV_LABELING='" & Common.Parse(pComp.PrivLabeling) & "'," &
        '            "CM_CONTACT='" & Common.Parse(pComp.ContactPerson) & "'," &
        '            "CM_SKINS_ID='" & Common.Parse(pComp.Skins) & "'," &
        '            "CM_TRAINING='" & Common.Parse(pComp.TrainDemo) & "'," &
        '            "CM_REPORT_USERS='" & IIf(Common.Parse(pComp.ReportUsers) = "", 0, Common.Parse(pComp.ReportUsers)) & "," &
        '            "CM_MOD_BY='" & ctx.Session("UserId") & "', " &
        '            "CM_MULTI_PO='" & Common.Parse(pComp.MultiInvAppr) & "'," &
        '            "CM_BA_CANCEL='" & Common.Parse(pComp.BACanPO) & "'," &
        '            "CM_MOD_DT=GETDATE()"

        '        ' ai chu add on 05/12/2005
        '        ' SR U30012 - to include additional fields to allow hub admin enter the 
        '        ' no of SKU and no of transacrion at the company detail screen
        '        If pComp.CoyType = "VENDOR" Or pComp.CoyType = "BOTH" Then
        '            strSql &= ", CM_SKU = " & IIf(Common.Parse(pComp.SKU) = "", "NULL", Common.Parse(pComp.SKU)) & ", CM_TRANS_NO = " & IIf(Common.Parse(pComp.TransNo) = "", "NULL", Common.Parse(pComp.TransNo)) & " "
        '        End If

        '        strSql &= "WHERE CM_COY_ID ='" & Common.Parse(pOriCoyId) & "'"

        '        If objDb.Execute(strSql) Then
        '            Message = Common.RecordSave
        '            Return True
        '        End If
        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        ''*************************************************************************************
        'Created By:  Esther
        'Date:  27/05/2005
        'Screen:  Company Details
        'Purpose:  to get the status description from status_mstr, company_mstr table
        '***************************************************************************************
        Public Function updateCompFlag(ByVal strValue As String)
            'update value into database table company_setting (cs_flag_value)

            Dim strSql As String
            Dim i As Integer
            Dim strAryQuery(0) As String
            'If objDb.Exist("SELECT '*' FROM COMPANY_SETTING WHERE CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CS_FLAG_NAME = 'Approval Rule') > 0 Then
            '    strSql = "UPDATE COMPANY_SETTING SET CS_FLAG_VALUE ='" & strValue & "' "
            '    strSql &= "where CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            '    strSql &= "AND CS_FLAG_NAME = 'Approval Rule' "
            'Else
            '    strSql = "INSERT INTO COMPANY_SETTING(CS_COY_ID, CS_FLAG_NAME, CS_FLAG_VALUE) VALUES ("
            '    strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
            '    strSql &= "'APPROVAL RULE', "
            '    strSql &= "'" & strValue & "' )"

            strSql = "delete from  COMPANY_SETTING where cs_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strAryQuery, strSql)


            strSql = "INSERT INTO COMPANY_SETTING(CS_COY_ID, CS_FLAG_NAME, CS_FLAG_VALUE) VALUES ("
            strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', 'APPROVAL RULE', "
            strSql &= "'" & strValue & "' )"
            Common.Insert2Ary(strAryQuery, strSql)

            objDb.BatchExecute(strAryQuery)

        End Function

        ''*************************************************************************************
        'Created By:  Ya Li
        'Date:  26/05/2005
        'Screen:  Public Vendor Registration
        'Purpose:  Update Public Vendor Registration details into CM_COMPANY_TEMP.
        '**************************************************************************************
        'Function UpdateDetails(ByVal pComp As Company) As Boolean

        '    Dim strSql As String
        '    strSql = "UPDATE COMPANY_MSTR_TEMP SET CM_COY_NAME='" & Common.Parse(pComp.CoyName) & "'," &
        '    "CM_STATUS='" & Common.Parse(pComp.Status) & "'," &
        '    "CM_COY_TYPE='" & Common.Parse(pComp.CoyType) & "'," &
        '    "CM_PARENT_COY_ID='" & Common.Parse(pComp.ParentCoy) & "'," &
        '    "CM_LICENCE_PACKAGE='" & Common.Parse(pComp.Package) & "', " &
        '    "CM_LICENSE_USERS='" & Common.Parse(pComp.LicenseUsers) & "', " &
        '    "CM_SUB_START_DT=" & Common.ConvertDate(pComp.SubStart) & ", " &
        '    "CM_SUB_END_DT=" & Common.ConvertDate(pComp.SubEnd) & ", " &
        '    "CM_ADDR_LINE1='" & Common.Parse(pComp.Address1) & "'," &
        '    "CM_ADDR_LINE2 ='" & Common.Parse(pComp.Address2) & "'," &
        '    "CM_ADDR_LINE3 ='" & Common.Parse(pComp.Address3) & "'," &
        '    "CM_CITY ='" & Common.Parse(pComp.City) & "'," &
        '    "CM_STATE='" & Common.Parse(pComp.State) & "'," &
        '    "CM_POSTCODE ='" & Common.Parse(pComp.PostCode) & "'," &
        '    "CM_COUNTRY ='" & Common.Parse(pComp.Country) & "'," &
        '    "CM_PHONE='" & Common.Parse(pComp.Phone) & "'," &
        '    "CM_FAX ='" & Common.Parse(pComp.Fax) & "'," &
        '    "CM_EMAIL ='" & Common.Parse(pComp.Email) & "'," &
        '    "CM_BUSINESS_REG_NO ='" & Common.Parse(pComp.BusinessRegNo) & "'," &
        '    "CM_ACCT_NO ='" & Common.Parse(pComp.AccountNo) & "'," &
        '    "CM_BANK='" & Common.Parse(pComp.BankCode) & "'," &
        '    "CM_BRANCH='" & Common.Parse(pComp.BranchCode) & "'," &
        '    "CM_CURRENCY_CODE='" & Common.Parse(pComp.Currency) & "', " &
        '    "CM_TAX_REG_NO ='" & Common.Parse(pComp.TaxRegNo) & "'," &
        '    "CM_TAX_CALC_BY='" & Common.Parse(pComp.TaxCalBy) & "'," &
        '    "CM_BCM_SET='" & Common.Parse(pComp.BCMSetting) & "'," &
        '    "CM_FINDEPT_MODE='" & Common.Parse(pComp.FinDeptMode) & "'," &
        '    "CM_INV_APPR='" & Common.Parse(pComp.InvAppr) & "'," &
        '    "CM_PAYMENT_TERM ='" & Common.Parse(pComp.PaymentTerm) & "'," &
        '    "CM_PAYMENT_METHOD ='" & Common.Parse(pComp.PaymentMethod) & "'," &
        '    "CM_PWD_DURATION=" & Common.Parse(pComp.PwdDuration) & "," &
        '    "CM_PRIV_LABELING='" & Common.Parse(pComp.PrivLabeling) & "'," &
        '    "CM_SKINS_ID='" & Common.Parse(pComp.Skins) & "'," &
        '    "CM_REPORT_USERS=" & Common.Parse(pComp.ReportUsers) & "," &
        '    "CM_MOD_BY='" & ctx.Session("UserId") & "'," &
        '    "CM_MULTI_PO='" & Common.Parse(pComp.MultiInvAppr) & "'," &
        '    "CM_BA_CANCEL='" & Common.Parse(pComp.BACanPO) & "'," &
        '    "CM_MOD_DT=GETDATE()"

        '    ' ai chu add on 05/12/2005
        '    ' SR U30012 - to include additional fields to allow hub admin enter the 
        '    ' no of SKU and no of transacrion at the company detail screen
        '    If pComp.CoyType = "VENDOR" Or pComp.CoyType = "BOTH" Then
        '        'strSql &= ", CM_SKU = " & pComp.SKU & ", CM_TRANS_NO = " & pComp.TransNo & " "
        '        strSql &= ", CM_SKU = " & IIf(Common.Parse(pComp.SKU) = "", "NULL", Common.Parse(pComp.SKU)) & ", CM_TRANS_NO = " & IIf(Common.Parse(pComp.TransNo) = "", "NULL", Common.Parse(pComp.TransNo)) & " "
        '    End If

        '    strSql &= "WHERE CM_COY_ID ='" & Common.Parse(pComp.CoyId) & "'"

        '    If objDb.Execute(strSql) Then
        '        Message = Common.RecordSave
        '        Return True
        '    End If
        'End Function

        Function UpdatePublicRegHub(ByVal pComp As Company, ByVal comID As String) As String

            Dim strSql, strSqlExist, strSqlDel As String

            strSqlExist = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
                  Common.Parse(comID) & "' AND CM_DELETED <>'Y'"

            strSqlDel = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
                   Common.Parse(comID) & "' AND CM_DELETED='Y'"
            Try
                If objDb.Exist(strSqlExist) Then
                    strCompMassage = Common.RecordDuplicate
                    Return 0
                ElseIf objDb.Exist(strSqlDel) Then
                    strCompMassage = Common.RecordUsed
                    Return 0
                Else
                    strSql = "INSERT INTO COMPANY_MSTR( " &
                    "CM_COY_ID, CM_COY_NAME, CM_COY_TYPE, CM_ACCT_NO,CM_BANK, CM_BRANCH, CM_ADDR_LINE1, " &
                    "CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, " &
                    "CM_EMAIL, CM_COY_LOGO, CM_BUSINESS_REG_NO, CM_TAX_REG_NO, CM_PAYMENT_TERM, CM_PAYMENT_METHOD, CM_ACTUAL_TERMSANDCONDFILE, CM_HUB_TERMSANDCONDFILE, CM_PWD_DURATION, CM_TAX_CALC_BY, " &
                    "CM_CURRENCY_CODE, CM_BCM_SET, CM_RFQ_OPTION, CM_LICENCE_PACKAGE, CM_LICENSE_USERS, CM_SUB_START_DT, CM_SUB_END_DT, CM_FINDEPT_MODE, CM_INV_APPR, CM_PRIV_LABELING, CM_SKINS_ID, " &
                    "CM_TRAINING, CM_STATUS, CM_DELETED, CM_MOD_BY, CM_MOD_DT, CM_SKU, CM_TRANS_NO, CM_CONTACT, CM_REPORT_USERS, CM_MULTI_PO) " &
                    "SELECT CM_COY_ID, CM_COY_NAME, CM_COY_TYPE, CM_ACCT_NO, CM_BANK, CM_BRANCH, CM_ADDR_LINE1, " &
                    "CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, " &
                    "CM_EMAIL, CM_COY_LOGO, CM_BUSINESS_REG_NO, CM_TAX_REG_NO, CM_PAYMENT_TERM, CM_PAYMENT_METHOD, CM_ACTUAL_TERMSANDCONDFILE, CM_HUB_TERMSANDCONDFILE, CM_PWD_DURATION, CM_TAX_CALC_BY, " &
                    "CM_CURRENCY_CODE, CM_BCM_SET, CM_RFQ_OPTION, CM_LICENCE_PACKAGE, CM_LICENSE_USERS, CM_SUB_START_DT, CM_SUB_END_DT, CM_FINDEPT_MODE, CM_INV_APPR, CM_PRIV_LABELING, CM_SKINS_ID, " &
                    "CM_TRAINING, CM_STATUS, CM_DELETED, CM_MOD_BY, CM_MOD_DT, CM_SKU, CM_TRANS_NO, CM_CONTACT, CM_REPORT_USERS, CM_MULTI_PO " &
                    "FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID='" & comID & "'"
                    'objDb.Execute(strSql)

                    If objDb.Execute(strSql) Then
                        Message = Common.RecordSave
                        Return 1
                    End If
                End If
            Catch errExp As CustomException
                Common.TrwExp(errExp)
            Catch errExp1 As Exception
                Common.TrwExp(errExp1)
            End Try
        End Function

        'Function updateStatusBuyerApproval(ByVal pCompID As String) As String

        '    Dim objDb As New EAD.DBCom
        '    Dim strsql As String

        '    strsql = "UPDATE COMPANY_MSTR_TEMP " &
        '                "SET CM_BUYER_APPR_STATUS='" & VendorRegApprStatus.Approved & "', " &
        '                "CM_BUYER_APPR_BY='" & ctx.Session("UserId") & "', " &
        '                "CM_BUYER_APPR_DATE = GETDATE()" &
        '                "WHERE CM_COY_ID='" & pCompID & "' "

        '    objDb.Execute(strsql)

        'End Function

        Function updateStatusBuyerReject(ByVal ComID As String, ByVal Status As String) As String

            Dim objDb As New EAD.DBCom
            Dim strsql As String

            strsql = "UPDATE COMPANY_MSTR_TEMP " &
                        "SET CM_BUYER_APPR_STATUS='" & Status & "' " &
                        "WHERE CM_COY_ID='" & Common.Parse(ComID) & "' "
            '"CM_HUB_APPR_BY = '" & ctx.Session("UserId") & "'" & _


            objDb.Execute(strsql)

        End Function

        ''*************************************************************************************
        'Created By:  Ya Li
        'Date:  26/05/2005
        'Screen:  Public Vendor Registration Approval
        'Purpose:  Update CM_APPR_STATUS in COMPANY_MSTR_TEMP.
        '************************************************************************************

        'Function updateStatusHubApproval(ByVal pCompID As String) As Boolean

        '    Dim objDb As New EAD.DBCom
        '    Dim strSQL, strSqlExist As String
        '    Dim strQuery(0) As String
        '    Dim strApp(0) As String

        '    strSQL = "UPDATE COMPANY_MSTR_TEMP " &
        '                " SET CM_HUB_APPR_STATUS='" & VendorRegApprStatus.Approved & "', " &
        '                " CM_HUB_APPR_BY='" & Common.Parse(ctx.Session("UserId")) & "', " &
        '                " CM_HUB_APPR_DATE = GETDATE() " &
        '                " WHERE CM_COY_ID='" & Common.Parse(pCompID) & "' "

        '    Common.Insert2Ary(strQuery, strSQL)

        '    strSqlExist = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
        '        Common.Parse(pCompID) & "' AND CM_DELETED <>'Y'"
        '    If objDb.Exist(strSqlExist) Then
        '        strCompMassage = Common.RecordDuplicate
        '        Return 0

        '    Else

        '        strSQL = "INSERT INTO COMPANY_MSTR (" &
        '                    "CM_COY_ID, CM_COY_NAME, CM_COY_TYPE, CM_PARENT_COY_ID, CM_ACCT_NO, CM_BANK, CM_BRANCH, CM_ADDR_LINE1, CM_ADDR_LINE2, " &
        '                    "CM_ADDR_LINE3, CM_POSTCODE, CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, CM_EMAIL, CM_COY_LOGO, " &
        '                    "CM_BUSINESS_REG_NO, CM_TAX_REG_NO, CM_PAYMENT_TERM, CM_PAYMENT_METHOD, CM_ACTUAL_TERMSANDCONDFILE, " &
        '                    "CM_HUB_TERMSANDCONDFILE, CM_PWD_DURATION, CM_TAX_CALC_BY, CM_CURRENCY_CODE, CM_BCM_SET, CM_BUDGET_FROM_DATE, " &
        '                    "CM_BUDGET_TO_DATE, CM_RFQ_OPTION, CM_LICENCE_PACKAGE, CM_LICENSE_USERS, CM_SUB_START_DT, CM_SUB_END_DT, " &
        '                    "CM_LICENSE_PRODUCTS, CM_FINDEPT_MODE, CM_INV_APPR, CM_PRIV_LABELING, CM_SKINS_ID, CM_TRAINING, CM_STATUS, CM_DELETED, CM_MOD_BY, " &
        '                    "CM_MOD_DT, CM_ENT_BY, CM_ENT_DT, CM_SKU, CM_TRANS_NO, CM_CONTACT, CM_REPORT_USERS, CM_MULTI_PO" &
        '                    ") " &
        '                    "SELECT CM_COY_ID, CM_COY_NAME, CM_COY_TYPE, CM_PARENT_COY_ID, CM_ACCT_NO, CM_BANK, CM_BRANCH, CM_ADDR_LINE1, CM_ADDR_LINE2, " &
        '                    "CM_ADDR_LINE3, CM_POSTCODE, CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, CM_EMAIL, CM_COY_LOGO, " &
        '                    "CM_BUSINESS_REG_NO, CM_TAX_REG_NO, CM_PAYMENT_TERM, CM_PAYMENT_METHOD, CM_ACTUAL_TERMSANDCONDFILE, " &
        '                    "CM_HUB_TERMSANDCONDFILE, CM_PWD_DURATION, CM_TAX_CALC_BY, CM_CURRENCY_CODE, CM_BCM_SET, CM_BUDGET_FROM_DATE, " &
        '                    "CM_BUDGET_TO_DATE, CM_RFQ_OPTION, CM_LICENCE_PACKAGE, CM_LICENSE_USERS, CM_SUB_START_DT, CM_SUB_END_DT, " &
        '                    "CM_LICENSE_PRODUCTS, CM_FINDEPT_MODE, CM_INV_APPR, CM_PRIV_LABELING, CM_SKINS_ID, CM_TRAINING, CM_STATUS, CM_DELETED, CM_MOD_BY, " &
        '                    "CM_MOD_DT, CM_ENT_BY, CM_ENT_DT, CM_SKU, CM_TRANS_NO, CM_CONTACT, CM_REPORT_USERS,CM_MULTI_PO " &
        '                    "FROM COMPANY_MSTR_TEMP " &
        '                    "WHERE CM_COY_ID = '" & Common.Parse(pCompID) & "'"

        '        Common.Insert2Ary(strQuery, strSQL)

        '        strSQL = "INSERT INTO COMPANY_APPLICATION (" &
        '                    "CA_COY_ID, CA_APP_ID" &
        '                    ") VALUES ('" &
        '                    Common.Parse(pCompID) & "','" &
        '                    EnumAppPackage.eRFP.ToString & "') "

        '        Common.Insert2Ary(strQuery, strSQL)
        '    End If

        '    Try
        '        objDb.BatchExecute(strQuery)
        '        Return True
        '    Catch ex As Exception
        '        Message = ex.Message
        '        Return False
        '    End Try
        'End Function

        'Function updateStatusHubReject(ByVal ComID As String, ByVal Status As String) As Boolean

        '    Dim objDb As New EAD.DBCom
        '    Dim strsql As String

        '    strsql = "UPDATE COMPANY_MSTR_TEMP " &
        '                " SET CM_HUB_APPR_STATUS='" & VendorRegApprStatus.Reject & "', " &
        '                " CM_MOD_BY='" & Common.Parse(ctx.Session("UserId")) & "', " &
        '                " CM_MOD_DT= GETDATE() " &
        '                " WHERE CM_COY_ID='" & Common.Parse(ComID) & "' "
        '    '"CM_HUB_APPR_BY = '" & ctx.Session("UserId") & "'" & _

        '    Try
        '        objDb.Execute(strsql)
        '        Return True
        '    Catch ex As Exception
        '        Message = ex.Message
        '        Return False
        '    End Try
        'End Function

        Public Function getHubApproveStatus(ByVal COY_ID As String) As Integer
            Dim strSQL, strStatus As String
            strSQL = "SELECT CM_HUB_APPR_STATUS FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID = '" & COY_ID & "'"
            strStatus = objDb.GetVal(strSQL)
            Return strStatus
        End Function

        Public Function GetTemporaryCompanyDetails(ByVal pCompId As String) As Company
            Dim strGet As String
            Dim dtCoy As DataTable
            Dim objCoyDetails As New Company
            strGet = "SELECT * FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID = '" & Common.Parse(pCompId) & "'"
            dtCoy = objDb.FillDt(strGet)

            If Not dtCoy Is Nothing Then
                With dtCoy.Rows(0)
                    objCoyDetails.CoyId = IIf(IsDBNull(.Item("CM_COY_ID")), "", .Item("CM_COY_ID"))
                    objCoyDetails.CoyName = IIf(IsDBNull(.Item("CM_COY_NAME")), "", .Item("CM_COY_NAME"))
                    objCoyDetails.ParentCoy = IIf(IsDBNull(.Item("CM_PARENT_COY_ID")), "", .Item("CM_PARENT_COY_ID"))
                    objCoyDetails.AccountNo = IIf(IsDBNull(.Item("CM_ACCT_NO")), "", .Item("CM_ACCT_NO"))
                    objCoyDetails.BankCode = IIf(IsDBNull(.Item("CM_BANK")), "", .Item("CM_BANK"))
                    objCoyDetails.BranchCode = IIf(IsDBNull(.Item("CM_BRANCH")), "", .Item("CM_BRANCH"))
                    objCoyDetails.Currency = IIf(IsDBNull(.Item("CM_CURRENCY_CODE")), "", .Item("CM_CURRENCY_CODE"))
                    objCoyDetails.Address1 = IIf(IsDBNull(.Item("CM_ADDR_LINE1")), "", .Item("CM_ADDR_LINE1"))
                    objCoyDetails.Address2 = IIf(IsDBNull(.Item("CM_ADDR_LINE2")), "", .Item("CM_ADDR_LINE2"))
                    objCoyDetails.Address3 = IIf(IsDBNull(.Item("CM_ADDR_LINE3")), "", .Item("CM_ADDR_LINE3"))
                    objCoyDetails.City = IIf(IsDBNull(.Item("CM_CITY")), "", .Item("CM_CITY"))
                    objCoyDetails.State = IIf(IsDBNull(.Item("CM_STATE")), "", .Item("CM_STATE"))
                    objCoyDetails.PostCode = IIf(IsDBNull(.Item("CM_POSTCODE")), "", .Item("CM_POSTCODE"))
                    objCoyDetails.Country = IIf(IsDBNull(.Item("CM_COUNTRY")), "", .Item("CM_COUNTRY"))
                    objCoyDetails.Phone = IIf(IsDBNull(.Item("CM_PHONE")), "", .Item("CM_PHONE"))
                    objCoyDetails.Fax = IIf(IsDBNull(.Item("CM_FAX")), "", .Item("CM_FAX"))
                    objCoyDetails.SubStart = IIf(IsDBNull(.Item("CM_SUB_START_DT")), "", .Item("CM_SUB_START_DT"))
                    objCoyDetails.SubEnd = IIf(IsDBNull(.Item("CM_SUB_END_DT")), "", .Item("CM_SUB_END_DT"))
                    objCoyDetails.Email = IIf(IsDBNull(.Item("CM_EMAIL")), "", .Item("CM_EMAIL"))
                    objCoyDetails.CoyLogo = IIf(IsDBNull(.Item("CM_COY_LOGO")), "", .Item("CM_COY_LOGO"))
                    objCoyDetails.BusinessRegNo = IIf(IsDBNull(.Item("CM_BUSINESS_REG_NO")), "", .Item("CM_BUSINESS_REG_NO"))
                    objCoyDetails.TaxRegNo = IIf(IsDBNull(.Item("CM_TAX_REG_NO")), "", .Item("CM_TAX_REG_NO"))
                    objCoyDetails.TaxCalBy = IIf(IsDBNull(.Item("CM_TAX_CALC_BY")), "", .Item("CM_TAX_CALC_BY"))
                    objCoyDetails.PaymentMethod = IIf(IsDBNull(.Item("CM_PAYMENT_METHOD")), "", .Item("CM_PAYMENT_METHOD"))
                    objCoyDetails.PaymentTerm = IIf(IsDBNull(.Item("CM_PAYMENT_TERM")), "", .Item("CM_PAYMENT_TERM"))
                    objCoyDetails.PwdDuration = IIf(IsDBNull(.Item("CM_PWD_DURATION")), "", .Item("CM_PWD_DURATION"))
                    objCoyDetails.Status = IIf(IsDBNull(.Item("CM_STATUS")), "", .Item("CM_STATUS"))
                    objCoyDetails.CoyType = IIf(IsDBNull(.Item("CM_COY_TYPE")), "", .Item("CM_COY_TYPE"))
                    objCoyDetails.BCMSetting = IIf(IsDBNull(.Item("CM_BCM_SET")), "", .Item("CM_BCM_SET"))
                    objCoyDetails.BCMStart = IIf(IsDBNull(.Item("CM_BUDGET_FROM_DATE")), "", .Item("CM_BUDGET_FROM_DATE"))
                    objCoyDetails.BCMEnd = IIf(IsDBNull(.Item("CM_BUDGET_TO_DATE")), "", .Item("CM_BUDGET_TO_DATE"))
                    objCoyDetails.FinDeptMode = IIf(IsDBNull(.Item("CM_FINDEPT_MODE")), "", .Item("CM_FINDEPT_MODE"))
                    objCoyDetails.InvAppr = IIf(IsDBNull(.Item("CM_INV_APPR")), "", .Item("CM_INV_APPR"))
                    objCoyDetails.PrivLabeling = IIf(IsDBNull(.Item("CM_PRIV_LABELING")), "", .Item("CM_PRIV_LABELING"))
                    objCoyDetails.TrainDemo = IIf(IsDBNull(.Item("CM_TRAINING")), "", .Item("CM_TRAINING"))
                    objCoyDetails.Skins = IIf(IsDBNull(.Item("CM_SKINS_ID")), "", .Item("CM_SKINS_ID"))
                    objCoyDetails.Actual_TC = IIf(IsDBNull(.Item("CM_ACTUAL_TERMSANDCONDFILE")), "", .Item("CM_ACTUAL_TERMSANDCONDFILE"))
                    objCoyDetails.TC = IIf(IsDBNull(.Item("CM_HUB_TERMSANDCONDFILE")), "", .Item("CM_HUB_TERMSANDCONDFILE"))
                    objCoyDetails.LicenseUsers = IIf(IsDBNull(.Item("CM_LICENSE_USERS")), "", .Item("CM_LICENSE_USERS"))
                    objCoyDetails.SKU = IIf(IsDBNull(.Item("CM_SKU")), "", .Item("CM_SKU"))
                    objCoyDetails.TransNo = IIf(IsDBNull(.Item("CM_TRANS_NO")), "", .Item("CM_TRANS_NO"))
                    objCoyDetails.ContactPerson = Common.parseNull(.Item("CM_CONTACT"))
                    objCoyDetails.ReportUsers = IIf(IsDBNull(.Item("CM_REPORT_USERS")), "", .Item("CM_REPORT_USERS"))
                    objCoyDetails.MultiInvAppr = IIf(IsDBNull(.Item("CM_MULTI_PO")), "", .Item("CM_MULTI_PO"))
                    objCoyDetails.BACanPO = IIf(IsDBNull(.Item("CM_BA_CANCEL")), "", .Item("CM_MULTI_PO"))
                End With
                GetTemporaryCompanyDetails = objCoyDetails
            Else
                GetTemporaryCompanyDetails = Nothing
            End If

        End Function

        ''*************************************************************************************
        'Created By:  Ya Li
        'Date: 26/05/2005
        'Screen:  Public Vendor Registration Approval
        'Purpose:  Insert new company details into COMPANY_MSTR.
        '**************************************************************************************
        Public Function AddInCompMSTR(ByVal comID As String) As Integer
            'ByVal pComp As Company
            Dim strSql, strSqlExist, strSqlDel As String

            strSqlExist = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
                  Common.Parse(comID) & "' AND CM_DELETED <>'Y'"

            strSqlDel = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" &
                   Common.Parse(comID) & "' AND CM_DELETED='Y'"
            Try

                If objDb.Exist(strSqlExist) Then
                    strCompMassage = Common.RecordDuplicate
                    Return 0
                ElseIf objDb.Exist(strSqlDel) Then
                    strCompMassage = Common.RecordUsed
                    Return 0
                Else
                    strSql = "INSERT INTO COMPANY_MSTR( " &
                    "CM_COY_ID, CM_COY_NAME, CM_COY_TYPE, CM_PARENT_COY_ID, CM_ACCT_NO,CM_BANK, CM_BRANCH, CM_ADDR_LINE1, " &
                    "CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, " &
                    "CM_EMAIL, CM_COY_LOGO, CM_BUSINESS_REG_NO, CM_TAX_REG_NO, CM_PAYMENT_TERM, CM_PAYMENT_METHOD, CM_ACTUAL_TERMSANDCONDFILE, " &
                    "CM_HUB_TERMSANDCONDFILE, CM_PWD_DURATION, CM_TAX_CALC_BY, CM_CURRENCY_CODE, CM_BCM_SET, CM_BUDGET_FROM_DATE, CM_BUDGET_TO_DATE, CM_RFQ_OPTION, " &
                    "CM_LICENCE_PACKAGE,CM_LICENSE_USERS,CM_SUB_START_DT,CM_SUB_END_DT,CM_LICENSE_PRODUCTS, " &
                    "CM_FINDEPT_MODE, CM_INV_APPR, CM_PRIV_LABELING, CM_SKINS_ID, CM_TRAINING, CM_STATUS, CM_DELETED, CM_MOD_BY, CM_MOD_DT, CM_ENT_BY, CM_ENT_DT, CM_SKU, CM_TRANS_NO, CM_CONTACT, CM_REPORT_USERS, CM_MULTI_PO) " &
                    "SELECT CM_COY_ID, CM_COY_NAME, CM_COY_TYPE, CM_PARENT_COY_ID, CM_ACCT_NO, CM_BANK, CM_BRANCH, CM_ADDR_LINE1, " &
                    "CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, " &
                    "CM_EMAIL, CM_COY_LOGO, CM_BUSINESS_REG_NO, CM_TAX_REG_NO, CM_PAYMENT_TERM, CM_PAYMENT_METHOD, CM_ACTUAL_TERMSANDCONDFILE," &
                    "CM_HUB_TERMSANDCONDFILE, CM_PWD_DURATION, CM_TAX_CALC_BY, CM_CURRENCY_CODE, CM_BCM_SET, CM_BUDGET_FROM_DATE, CM_BUDGET_TO_DATE, CM_RFQ_OPTION, " &
                    "CM_LICENCE_PACKAGE,CM_LICENSE_USERS,CM_SUB_START_DT,CM_SUB_END_DT,CM_LICENSE_PRODUCTS, " &
                    "CM_FINDEPT_MODE, CM_INV_APPR, CM_PRIV_LABELING, CM_SKINS_ID, CM_TRAINING, CM_STATUS, CM_DELETED, CM_MOD_BY, CM_MOD_DT, CM_ENT_BY, CM_ENT_DT, CM_SKU, CM_TRANS_NO, CM_CONTACT, CM_REPORT_USERS, CM_MULTI_PO " &
                    "FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID='" & comID & "'"

                    If objDb.Execute(strSql) Then
                        Message = Common.RecordSave
                        Return 1
                    End If
                End If
            Catch errExp As CustomException
                Common.TrwExp(errExp)
            Catch errExp1 As Exception
                Common.TrwExp(errExp1)
            End Try
        End Function

        Public Function getBuyerApproveStatus(ByVal COY_ID As String) As Integer
            Dim strSQL, strStatus As String
            strSQL = "SELECT CM_BUYER_APPR_STATUS FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID = '" & COY_ID & "'"
            strStatus = objDb.GetVal(strSQL)
            Return strStatus
        End Function

        ''*************************************************************************************
        'Created By:  Esther
        'Date:  27/05/2005
        'Screen:  Company Details
        'script:  CompanyDetail.aspx
        'Purpose:  to get the status description from status_mstr, company_mstr table
        '***************************************************************************************


        Public Function GetCompFlag(Optional ByVal strCoyID As String = "") As DataSet
            Dim strGet As String
            Dim dsCoy As DataSet

            If strCoyID = "" Then
                strCoyID = HttpContext.Current.Session("CompanyID")
            End If

            strGet = "select status_no, status_desc, 'Y' as chk from company_setting"
            strGet &= " left join status_mstr on status_no= cs_flag_value where cs_coy_id = '" & strCoyID & "' "
            strGet &= " and status_type = 'APPROVAL RULE' AND CS_APP_PKG = 'eRFP' union select status_no, status_desc, 'N' as chk from status_mstr "
            strGet &= "where status_type = 'APPROVAL RULE' and status_no not in (select cs_flag_value from  "
            strGet &= " company_setting where cs_coy_id = '" & strCoyID & "' AND CS_APP_PKG = 'eRFP')"


            dsCoy = objDb.FillDs(strGet)
            GetCompFlag = dsCoy


        End Function

        ''*************************************************************************************
        'Created By:  Ya Li
        'Date:  25/05/2005
        'Screen:  Public Vendor Registration Approval
        'Purpose:  Get data from COMPANY_MSTR_TEMP for viewing or modify.
        '**************************************************************************************
        Public Function GetDetails(ByVal ComID As String) As Company
            Dim strGet As String
            Dim dtCoy As DataTable
            Dim objCoyDetails As New Company

            strGet = "SELECT *, " &
                     "(SELECT CODE_DESC FROM CODE_MSTR WHERE CM_STATE=CODE_ABBR AND CODE_CATEGORY='S' AND CM_COUNTRY=CODE_VALUE AND CODE_VALUE='MY') AS STATE, " &
                     "(SELECT CODE_DESC FROM CODE_MSTR WHERE CM_COUNTRY=CODE_ABBR AND CODE_CATEGORY='CT') AS COUNTRY, " &
                     "(SELECT CODE_DESC FROM CODE_MSTR WHERE CM_CURRENCY_CODE=CODE_ABBR AND CODE_CATEGORY='CU') AS CURRENCY " &
                     "FROM COMPANY_MSTR_TEMP " &
                     "WHERE CM_COY_ID = '" & ComID & "' "
            dtCoy = objDb.FillDt(strGet)
            Dim status As String
            If Not dtCoy Is Nothing Then
                With dtCoy.Rows(0)
                    objCoyDetails.CoyId = IIf(IsDBNull(.Item("CM_COY_ID")), "", .Item("CM_COY_ID"))
                    objCoyDetails.CoyName = IIf(IsDBNull(.Item("CM_COY_NAME")), "", .Item("CM_COY_NAME"))
                    objCoyDetails.ParentCoy = IIf(IsDBNull(.Item("CM_PARENT_COY_ID")), "", .Item("CM_PARENT_COY_ID"))
                    objCoyDetails.AccountNo = IIf(IsDBNull(.Item("CM_ACCT_NO")), "", .Item("CM_ACCT_NO"))
                    objCoyDetails.BankCode = IIf(IsDBNull(.Item("CM_BANK")), "", .Item("CM_BANK"))
                    objCoyDetails.BranchCode = IIf(IsDBNull(.Item("CM_BRANCH")), "", .Item("CM_BRANCH"))
                    objCoyDetails.Currency = IIf(IsDBNull(.Item("CM_CURRENCY_CODE")), "", .Item("CM_CURRENCY_CODE"))
                    objCoyDetails.Address1 = IIf(IsDBNull(.Item("CM_ADDR_LINE1")), "", .Item("CM_ADDR_LINE1"))
                    objCoyDetails.Address2 = IIf(IsDBNull(.Item("CM_ADDR_LINE2")), "", .Item("CM_ADDR_LINE2"))
                    objCoyDetails.Address3 = IIf(IsDBNull(.Item("CM_ADDR_LINE3")), "", .Item("CM_ADDR_LINE3"))
                    objCoyDetails.City = IIf(IsDBNull(.Item("CM_CITY")), "", .Item("CM_CITY"))
                    objCoyDetails.State = IIf(IsDBNull(.Item("CM_STATE")), "", .Item("CM_STATE"))
                    objCoyDetails.PostCode = IIf(IsDBNull(.Item("CM_POSTCODE")), "", .Item("CM_POSTCODE"))
                    objCoyDetails.Country = IIf(IsDBNull(.Item("CM_COUNTRY")), "", .Item("CM_COUNTRY"))
                    objCoyDetails.Phone = IIf(IsDBNull(.Item("CM_PHONE")), "", .Item("CM_PHONE"))
                    objCoyDetails.Fax = IIf(IsDBNull(.Item("CM_FAX")), "", .Item("CM_FAX"))
                    objCoyDetails.SubStart = IIf(IsDBNull(.Item("CM_SUB_START_DT")), "", .Item("CM_SUB_START_DT"))
                    objCoyDetails.SubEnd = IIf(IsDBNull(.Item("CM_SUB_END_DT")), "", .Item("CM_SUB_END_DT"))
                    objCoyDetails.Email = IIf(IsDBNull(.Item("CM_EMAIL")), "", .Item("CM_EMAIL"))
                    objCoyDetails.CoyLogo = IIf(IsDBNull(.Item("CM_COY_LOGO")), "", .Item("CM_COY_LOGO"))
                    objCoyDetails.BusinessRegNo = IIf(IsDBNull(.Item("CM_BUSINESS_REG_NO")), "", .Item("CM_BUSINESS_REG_NO"))
                    objCoyDetails.TaxRegNo = IIf(IsDBNull(.Item("CM_TAX_REG_NO")), "", .Item("CM_TAX_REG_NO"))
                    objCoyDetails.GSTDateLastStatus = IIf(IsDBNull(.Item("CM_LAST_DATE")), "", .Item("CM_LAST_DATE"))
                    objCoyDetails.TaxCalBy = IIf(IsDBNull(.Item("CM_TAX_CALC_BY")), "", .Item("CM_TAX_CALC_BY"))
                    objCoyDetails.PaymentMethod = IIf(IsDBNull(.Item("CM_PAYMENT_METHOD")), "", .Item("CM_PAYMENT_METHOD"))
                    objCoyDetails.PaymentTerm = IIf(IsDBNull(.Item("CM_PAYMENT_TERM")), "", .Item("CM_PAYMENT_TERM"))
                    objCoyDetails.PwdDuration = IIf(IsDBNull(.Item("CM_PWD_DURATION")), "", .Item("CM_PWD_DURATION"))
                    'objCoyDetails.Status = status
                    'status = Common.parseNull(.Item("CM_STATUS"))
                    objCoyDetails.Status = Common.parseNull(.Item("CM_STATUS"))
                    objCoyDetails.CoyType = IIf(IsDBNull(.Item("CM_COY_TYPE")), "", .Item("CM_COY_TYPE"))
                    objCoyDetails.BCMSetting = Common.parseNull(.Item("CM_BCM_SET"))
                    objCoyDetails.BCMStart = IIf(IsDBNull(.Item("CM_BUDGET_FROM_DATE")), "", .Item("CM_BUDGET_FROM_DATE"))
                    objCoyDetails.BCMEnd = IIf(IsDBNull(.Item("CM_BUDGET_TO_DATE")), "", .Item("CM_BUDGET_TO_DATE"))
                    objCoyDetails.FinDeptMode = Common.parseNull(.Item("CM_FINDEPT_MODE"))
                    objCoyDetails.InvAppr = Common.parseNull(.Item("CM_INV_APPR"))
                    objCoyDetails.MultiInvAppr = Common.parseNull(.Item("CM_MULTI_PO"))
                    objCoyDetails.BACanPO = Common.parseNull(.Item("CM_BA_CANCEL"))
                    objCoyDetails.PrivLabeling = Common.parseNull(.Item("CM_PRIV_LABELING"))
                    objCoyDetails.TrainDemo = IIf(IsDBNull(.Item("CM_TRAINING")), "", .Item("CM_TRAINING"))
                    objCoyDetails.Skins = IIf(IsDBNull(.Item("CM_SKINS_ID")), "", .Item("CM_SKINS_ID"))
                    objCoyDetails.Actual_TC = IIf(IsDBNull(.Item("CM_ACTUAL_TERMSANDCONDFILE")), "", .Item("CM_ACTUAL_TERMSANDCONDFILE"))
                    objCoyDetails.TC = IIf(IsDBNull(.Item("CM_HUB_TERMSANDCONDFILE")), "", .Item("CM_HUB_TERMSANDCONDFILE"))
                    objCoyDetails.LicenseUsers = IIf(IsDBNull(.Item("CM_LICENSE_USERS")), "", .Item("CM_LICENSE_USERS"))
                    objCoyDetails.Package = IIf(IsDBNull(.Item("CM_LICENCE_PACKAGE")), "", .Item("CM_LICENCE_PACKAGE"))
                    objCoyDetails.SKU = IIf(IsDBNull(.Item("CM_SKU")), "", .Item("CM_SKU"))
                    objCoyDetails.TransNo = IIf(IsDBNull(.Item("CM_TRANS_NO")), "", .Item("CM_TRANS_NO"))
                    objCoyDetails.ContactPerson = Common.parseNull(.Item("CM_CONTACT"))
                    objCoyDetails.ReportUsers = IIf(IsDBNull(.Item("CM_REPORT_USERS")), "", .Item("CM_REPORT_USERS"))
                End With
                GetDetails = objCoyDetails
            Else
                GetDetails = Nothing
            End If

        End Function

        Public Function AddCompSetting(ByVal strFlagValue As String) As Boolean

            Dim strSQL, strCompId, Query(0) As String
            Dim i, j As Integer
            Dim strValue() As String



            strSQL = "delete from  COMPANY_SETTING where cs_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSQL &= "AND CS_APP_PKG = 'eRFP'"
            Common.Insert2Ary(Query, strSQL)

            If strFlagValue.Length > 0 Then
                strValue = strFlagValue.Split(",")
                For i = 0 To strValue.Length - 1
                    strSQL = "INSERT INTO COMPANY_SETTING(CS_COY_ID,CS_FLAG_NAME,CS_FLAG_VALUE,CS_FLAG_TYPE,CS_APP_PKG) " &
                    "VALUES ('" &
                    Common.Parse(HttpContext.Current.Session("CompanyId")) & "','APPROVAL RULE'," &
                    strValue(i) & " , "

                    Select Case strValue(i)
                        Case 0
                            strSQL &= "'PUBLISH',"
                        Case 1
                            strSQL &= "'AWARD',"
                        Case 2
                            strSQL &= "'MAINDOC',"
                    End Select
                    strSQL &= "'eRFP')"
                    Common.Insert2Ary(Query, strSQL)
                Next
            End If

            Return objDb.BatchExecute(Query)
        End Function

        Public Function GetBankName(ByVal strBankCode As String) As String
            Dim strSQL, strBankName As String
            strSQL = "SELECT BC_BANK_NAME FROM BANK_CODE WHERE BC_BANK_CODE = '" & Common.Parse(strBankCode) & "'"
            strBankName = objDb.GetVal(strSQL)
            Return strBankName
        End Function

        'Function UpdateIPPCompanyGSTRegNo(ByVal pComp As Company, ByVal strInd As Integer) As Integer
        '    If strInd <> 0 Then
        '        Dim strSql As String
        '        strSql = "SELECT * FROM INVOICE_MSTR " &
        '                "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
        '                "WHERE IM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'  AND ID_S_COY_ID = '" & Common.Parse(strInd) & "' AND (IM_INVOICE_STATUS NOT IN (4,14,15) OR ((IM_INVOICE_STATUS = 14 AND IM_ROUTE_TO IS NOT NULL)))"

        '        If objDb.Exist(strSql) <> 0 Then
        '            Return -99
        '        End If

        '        strSql = "UPDATE IPP_COMPANY SET IC_TAX_REG_NO ='" & Common.Parse(pComp.TaxRegNo) & "', " &
        '                "IC_MOD_BY='" & ctx.Session("UserId") & "'," &
        '                "IC_MOD_DATETIME= NOW() " &
        '                "WHERE IC_INDEX = '" & Common.Parse(strInd) & "' AND IC_COY_ID ='" & Common.Parse(pComp.CoyId) & "'"

        '        If objDb.Execute(strSql) Then
        '            Return WheelMsgNum.Save
        '        Else
        '            Return WheelMsgNum.NotSave
        '        End If
        '    End If
        'End Function

        'Function UpdateIPPCompany(ByVal pComp As Company, ByVal strCoyType As String, ByVal strFieldtoCheck As String, ByVal strInd As Integer) As Integer
        '    Dim strSql As String

        '    If strCoyType = "V" Then
        '        strSql = "SELECT * FROM invoice_mstr " &
        '                "INNER JOIN invoice_details ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " &
        '                "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'  AND  id_s_coy_id = '" & Common.Parse(strInd) & "' AND (im_invoice_status NOT IN (4,14,15) OR ((im_invoice_status = 14 AND im_route_to IS NOT NULL)))"

        '    Else
        '        strSql = "SELECT * FROM invoice_mstr " &
        '                "INNER JOIN invoice_details ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " &
        '                "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'  AND id_pay_for = '" & Common.Parse(pComp.BOtherCoy) & "' AND (im_invoice_status NOT IN (4,14,15) OR ((im_invoice_status = 14 AND im_route_to IS NOT NULL)))"

        '    End If

        '    'Zulham 05122018
        '    If objDb.Exist(strSql) <> 0 Then
        '        'Return -99
        '    End If

        '    If strCoyType = "V" Then
        '        strSql = "UPDATE IPP_COMPANY SET IC_COY_NAME='" & Common.Parse(pComp.CoyName) & "'," &
        '                    "IC_COY_TYPE='" & Common.Parse(pComp.CoyType) & "'," &
        '                    "IC_STATUS='" & Common.Parse(pComp.Status) & "'," &
        '                    "IC_BUSINESS_REG_NO='" & Common.Parse(pComp.BusinessRegNo) & "', " &
        '                    "IC_ADDR_LINE1='" & Common.Parse(pComp.Address1) & "'," &
        '                    "IC_ADDR_LINE2 ='" & Common.Parse(pComp.Address2) & "'," &
        '                    "IC_ADDR_LINE3 ='" & Common.Parse(pComp.Address3) & "'," &
        '                    "IC_POSTCODE ='" & Common.Parse(pComp.PostCode) & "'," &
        '                    "IC_CITY ='" & Common.Parse(pComp.City) & "'," &
        '                    "IC_STATE='" & Common.Parse(pComp.State) & "'," &
        '                    "IC_COUNTRY ='" & Common.Parse(pComp.Country) & "'," &
        '                    "IC_CONTACT ='" & Common.Parse(pComp.ContactPerson) & "'," &
        '                    "IC_PHONE='" & Common.Parse(pComp.Phone) & "'," &
        '                    "IC_FAX ='" & Common.Parse(pComp.Fax) & "'," &
        '                    "IC_EMAIL ='" & Common.Parse(pComp.Email) & "'," &
        '                    "IC_WEBSITE ='" & Common.Parse(pComp.WebSites) & "'," &
        '                    "IC_BANK_CODE ='" & Common.Parse(pComp.BankCode) & "'," &
        '                    "IC_BANK_ACCT ='" & Common.Parse(pComp.AccountNo) & "'," &
        '                    "IC_BANK_ADDR_LINE1 ='" & Common.Parse(pComp.BankAddrLine1) & "'," &
        '                    "IC_BANK_ADDR_LINE2 ='" & Common.Parse(pComp.BankAddrLine2) & "'," &
        '                    "IC_BANK_ADDR_LINE3 ='" & Common.Parse(pComp.BankAddrLine3) & "'," &
        '                    "IC_BANK_POSTCODE ='" & Common.Parse(pComp.BankPostcode) & "'," &
        '                    "IC_BANK_CITY ='" & Common.Parse(pComp.BankCity) & "'," &
        '                    "IC_BANK_STATE ='" & Common.Parse(pComp.BankState) & "'," &
        '                    "IC_BANK_COUNTRY ='" & Common.Parse(pComp.BankCountry) & "'," &
        '                    "IC_PAYMENT_METHOD ='" & Common.Parse(pComp.PaymentMethod) & "'," &
        '                    "IC_CON_IBS_CODE ='" & Common.Parse(pComp.ConIBSGLCode) & "'," &
        '                    "IC_NON_CON_IBS_CODE ='" & Common.Parse(pComp.NonConIBSGLCode) & "',"

        '        If pComp.CoyType = "V" Then
        '            strSql &= "IC_COMPANY_CATEGORY ='" & Common.Parse(pComp.CompanyCategory) & "'," &
        '                    "IC_RESIDENT_TYPE ='" & Common.Parse(pComp.ResidentType) & "'," &
        '                    "IC_RESIDENT_COUNTRY ='" & Common.Parse(pComp.ResidentCountry) & "'," &
        '                    "IC_TAX_REG_NO ='" & Common.Parse(pComp.TaxRegNo) & "'," &
        '                    "IC_LAST_DATE =" & IIf(pComp.GSTDateLastStatus = "", "NULL", Common.ConvertDate(pComp.GSTDateLastStatus)) & "," &
        '                    "IC_GST_INPUT_TAX_CODE ='" & Common.Parse(pComp.GstInputTaxCode) & "'," &
        '                    "IC_GST_OUTPUT_TAX_CODE ='" & Common.Parse(pComp.GstOutputTaxCode) & "'," &
        '                    "IC_GST_EFF_DATE =" & IIf(pComp.GstRegDate = "", "NULL", Common.ConvertDate(pComp.GstRegDate)) & "," &
        '                    "IC_NOSTRO_FLAG = '" & pComp.NostroIncome & "'," &
        '                    "IC_BILL_GL_CODE = '" & Common.Parse(pComp.BillGLCode) & "'," &
        '                    "IC_SYSTEM_VALID_DATE =" & IIf(pComp.SysValDate = "", "NULL", Common.ConvertDate(pComp.SysValDate)) & ","
        '            'Modified for IPP Gst Stage 2A 
        '            'Jules 2015.08.14 - Added IC_SYSTEM_VALID_DATE field for IPP Stage 4 Phase 2
        '        End If

        '        If pComp.CoyType = "E" Then
        '            'Zulham 29062015 - HLB-IPP Stage 4 (CR)
        '            strSql &= "IC_ADDITIONAL_1 ='" & Common.Parse(pComp.JobGrade) & "'," &
        '                    "IC_ADDITIONAL_2 =" & IIf(pComp.StaffCessationEffectiveDate = "", "NULL", Common.ConvertDate(pComp.StaffCessationEffectiveDate)) & "," &
        '                    "IC_ADDITIONAL_3 ='" & Common.Parse(pComp.BranchCode) & "'," &
        '                    "IC_ADDITIONAL_4 ='" & Common.Parse(pComp.CostCentre) & "'," &
        '                    "IC_GST_INPUT_TAX_CODE ='" & Common.Parse(pComp.GstInputTaxCode) & "'," &
        '                    "IC_GST_OUTPUT_TAX_CODE ='" & Common.Parse(pComp.GstOutputTaxCode) & "',"
        '        End If

        '        strSql &= "IC_MOD_BY='" & ctx.Session("UserId") & "'," &
        '                    "IC_MOD_DATETIME= NOW()," &
        '                    "IC_WAIVE_CHARGES = '" & Common.Parse(Common.parseNull(pComp.WaiveCharges)) & "'," &
        '                    "IC_CREDIT_TERMS = '" & Common.Parse(Common.parseNull(pComp.CreditTerms)) & "'," &
        '                    "IC_REMARK = '" & Common.Parse(Common.parseNull(Common.parseNull(pComp.InactiveReason))) & "'"

        '    Else
        '        strSql = "UPDATE IPP_COMPANY SET IC_COY_NAME='" & Common.Parse(pComp.CoyName) & "'," &
        '    "IC_COY_TYPE='" & Common.Parse(pComp.CoyType) & "'," &
        '    "IC_STATUS='" & Common.Parse(pComp.Status) & "'," &
        '    "IC_BUSINESS_REG_NO='" & Common.Parse(pComp.BusinessRegNo) & "', " &
        '    "IC_ADDR_LINE1='" & Common.Parse(pComp.Address1) & "'," &
        '    "IC_ADDR_LINE2 ='" & Common.Parse(pComp.Address2) & "'," &
        '    "IC_ADDR_LINE3 ='" & Common.Parse(pComp.Address3) & "'," &
        '    "IC_POSTCODE ='" & Common.Parse(pComp.PostCode) & "'," &
        '    "IC_CITY ='" & Common.Parse(pComp.City) & "'," &
        '    "IC_STATE='" & Common.Parse(pComp.State) & "'," &
        '    "IC_COUNTRY ='" & Common.Parse(pComp.Country) & "'," &
        '    "IC_CONTACT ='" & Common.Parse(pComp.ContactPerson) & "'," &
        '    "IC_PHONE='" & Common.Parse(pComp.Phone) & "'," &
        '    "IC_FAX ='" & Common.Parse(pComp.Fax) & "'," &
        '    "IC_EMAIL ='" & Common.Parse(pComp.Email) & "'," &
        '    "IC_WEBSITE ='" & Common.Parse(pComp.WebSites) & "'," &
        '    "IC_BANK_CODE ='" & Common.Parse(pComp.BankCode) & "'," &
        '    "IC_BANK_ACCT ='" & Common.Parse(pComp.AccountNo) & "'," &
        '    "IC_BANK_ADDR_LINE1 ='" & Common.Parse(pComp.BankAddrLine1) & "'," &
        '    "IC_BANK_ADDR_LINE2 ='" & Common.Parse(pComp.BankAddrLine2) & "'," &
        '    "IC_BANK_ADDR_LINE3 ='" & Common.Parse(pComp.BankAddrLine3) & "'," &
        '    "IC_BANK_POSTCODE ='" & Common.Parse(pComp.BankPostcode) & "'," &
        '    "IC_BANK_CITY ='" & Common.Parse(pComp.BankCity) & "'," &
        '    "IC_BANK_STATE ='" & Common.Parse(pComp.BankState) & "'," &
        '    "IC_BANK_COUNTRY ='" & Common.Parse(pComp.BankCountry) & "'," &
        '    "IC_PAYMENT_METHOD ='" & Common.Parse(pComp.PaymentMethod) & "'," &
        '    "IC_CON_IBS_CODE ='" & Common.Parse(pComp.ConIBSGLCode) & "'," &
        '    "IC_NON_CON_IBS_CODE ='" & Common.Parse(pComp.NonConIBSGLCode) & "'," &
        '    "IC_MOD_BY='" & ctx.Session("UserId") & "'," &
        '    "IC_MOD_DATETIME= NOW()," &
        '    "IC_REMARK = '" & Common.Parse(Common.parseNull(Common.parseNull(pComp.InactiveReason))) & "'"
        '    End If


        '    If strCoyType = "B" Then
        '        strSql &= " ,IC_OTHER_B_COY_CODE ='" & Common.Parse(pComp.BOtherCoy) & "',"
        '        strSql &= "IC_CURRENCY = '" & Common.parseNull(pComp.Currency) & "'"
        '    End If
        '    'WHERE CM_COY_ID ='" & Common.Parse(pComp.CoyId) & "'"

        '    ' ai chu add on 05/12/2005
        '    ' SR U30012 - to include additional fields to allow hub admin enter the 
        '    ' no of SKU and no of transacrion at the company detail screen
        '    'If pComp.CoyType = "VENDOR" Or pComp.CoyType = "BOTH" Then
        '    '    'strSql &= ", CM_SKU = " & pComp.SKU & ", CM_TRANS_NO = " & pComp.TransNo & " "
        '    '    strSql &= ", CM_SKU = " & IIf(Common.Parse(pComp.SKU) = "", "NULL", Common.Parse(pComp.SKU)) & ", CM_TRANS_NO = " & IIf(Common.Parse(pComp.TransNo) = "", "NULL", Common.Parse(pComp.TransNo)) & " "
        '    'End If

        '    'If strCoyType = "B" Then
        '    '    strSql &= " WHERE IC_OTHER_B_COY_CODE ='" & Common.Parse(strFieldtoCheck) & "'"
        '    'Else
        '    '    strSql &= " WHERE IC_COY_NAME ='" & Common.Parse(strFieldtoCheck) & "'"
        '    'End If

        '    'filter by ic_index instead of coy_name (vendor) or coy_code (buyer)
        '    strSql &= " WHERE IC_INDEX = '" & Common.Parse(strInd) & "' AND IC_COY_ID ='" & Common.Parse(pComp.CoyId) & "'"

        '    If objDb.Execute(strSql) Then
        '        '    If blnSetting Then
        '        '        AddCompSetting(strFlagValue)
        '        '        Message = Common.RecordSave
        '        '    End If
        '        'UpdateIPPCompany = WheelMsgNum.Save
        '        Return WheelMsgNum.Save

        '    Else
        '        'UpdateIPPCompany = WheelMsgNum.NotSave
        '        Return WheelMsgNum.NotSave

        '    End If
        'End Function

        '21/08/2014 - Chee Hong - To delta company related document from table temp
        Public Sub deleteCompDocAttach(ByVal strCompId As String, Optional ByVal strConnStr As String = Nothing)
            If strConnStr Is Nothing Then
                objDb = New EAD.DBCom
            Else
                objDb = New EAD.DBCom(strConnStr)
            End If

            Dim strsql As String

            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP " &
                    "WHERE CDA_COY_ID = '" & strCompId & "' AND CDA_DOC_TYPE = 'Company' "
            objDb.Execute(strsql)

        End Sub

        '21/08/2014 - Chee Hong - To insert company related document from table to table temp
        Public Sub insertCompDocAttach(ByVal strCompId As String, Optional ByVal strConnStr As String = Nothing)
            If strConnStr Is Nothing Then
                objDb = New EAD.DBCom
            Else
                objDb = New EAD.DBCom(strConnStr)
            End If

            Dim strsql As String

            'INSERT COMPANY_DOC_ATTACHMENT TABLE
            strsql = "INSERT INTO COMPANY_DOC_ATTACHMENT_TEMP(CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS) " &
                    "SELECT CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS FROM COMPANY_DOC_ATTACHMENT " &
                    "WHERE CDA_COY_ID = '" & strCompId & "' AND CDA_DOC_TYPE = 'Company'"
            objDb.Execute(strsql)

        End Sub

        '20/08/2014 - Chee Hong - eAdmin: To get company related document attachment
        Public Function getCompDocAttach(ByVal strCompId As String, Optional ByVal strConnStr As String = Nothing) As DataSet
            If strConnStr Is Nothing Then
                objDb = New EAD.DBCom
            Else
                objDb = New EAD.DBCom(strConnStr)
            End If

            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT_TEMP WHERE CDA_DOC_TYPE = 'Company' AND CDA_COY_ID = '" & strCompId & "'"

            ds = objDb.FillDs(strsql)
            getCompDocAttach = ds
        End Function

        Public Sub saveCompDocAttach(ByVal strCompId As String, Optional ByVal strConnStr As String = Nothing)
            Dim strSql As String
            If strConnStr Is Nothing Then
                objDb = New EAD.DBCom
            Else
                objDb = New EAD.DBCom(strConnStr)
            End If

            ' delete COMPANY_DOC_ATTACHMENT table
            strSql = "DELETE FROM COMPANY_DOC_ATTACHMENT " & _
                    "WHERE CDA_COY_ID = '" & strCompId & "' " & _
                    "AND CDA_DOC_TYPE = 'Company' "
            objDb.Execute(strSql)

            ' Insert COMPANY_DOC_ATTACHMENT table
            strSql = "INSERT INTO company_doc_attachment(CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS) "
            strSql &= "SELECT CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS FROM company_doc_attachment_temp "
            strSql &= "WHERE CDA_COY_ID = '" & strCompId & "' "
            strSql &= "AND CDA_DOC_TYPE = 'Company' "
            objDb.Execute(strSql)

        End Sub
#End Region
    End Class

End Namespace

