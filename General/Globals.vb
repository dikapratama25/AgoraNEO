Imports System
Imports System.Text
Imports System.IO
Imports System.Web
Imports System.Configuration
Imports System.Web.UI.WebControls
Imports System.Diagnostics
Imports CryptoClass
Imports System.Security.Cryptography
'Imports System.Security.Cryptography

Namespace AgoraNEO

    Public Enum FixedRole
        Approving_Officer
        Buyer
        Buyer_Administrator
        Consolidator
        Finance_Manager
        Finance_Officer
        Purchasing_Manager
        Purchasing_Officer
        Quality_Control_Officer
        Requester
        Second_Level_Receiver
        Store_Keeper
        Vendor
        Vendor_Administrator
        All_Role

        'eRFP - role

        Financial_Officer
        Technical_Officer
        RFP_Manager
        RFP_Admin
        RFP_Secretary
        RFP_Consultant
        Quantity_Surveyor
        Witnessing_Officer
        RFP_Vendor
        RFP_Vendor_Admin

        ' Super Admin
        Super_Admin
        Report_Administrator

        'IPP - Role
        IPP_Officer
        IPP_OfficerF

        'Billing - Role
        Billing_Officer
        Billing_OfficerF
    End Enum

    Public Enum AccessRight
        Screen
        FixedRole
    End Enum

    Public Enum CodeTable
        Country
        State
        Currency
        Uom
        Gst
        PaymentTerm
        ShipmentMode
        ShipmentTerm
        PaymentMethod
        MgmtCode
        ApprovalGroup
        Business
        OwnerShip
        IPPPaymentMethod
        GRNCtrlDays
        IPPCompanyCategory
    End Enum

    Public Enum invStatus
        NewInv = 1
        PendingAppr = 2
        Approved = 3
        Paid = 4

        Hold = 5
    End Enum

    'Chee Hong 2015.02.02 Agora Stage 2
    Public Enum DNStatus
        NewDN = 1
        PendingAppr = 2
        Approved = 3
        Paid = 4
    End Enum

    Public Enum CNStatus
        NewCN = 1
        AckCN = 2
    End Enum
    '-------------------------------------

    'Chee Hong 2015.11.16 Agora Staff Claim Module
    Public Enum SCStatus
        DraftSC = 1
        Submitted = 2
        PendingAppr = 3
        Approved = 4
        Rejected = 5
    End Enum

    Public Enum IPPStatus
        Draft = 10
        Submitted = 11
        PendingAppr = 12
        Approved = 13
        Rejected = 14
        Void = 15
    End Enum

    Public Enum PRStatus
        Draft = 1 'Open
        Submitted = 2 'ReleaseForApproval
        PendingApproval = 3
        Approved = 4
        ConvertedToPO = 5 '4
        CancelledBy = 6 '5
        HeldBy = 7 '6
        RejectedBy = 8
        Void = 9
    End Enum

    Public Enum POStatus 'old status
        NewPO = 1 '1
        AcceptedByVendor = 2 '2
        RejectedByVendor = 3 '3
        CancelledBY = 4 '4
        Invoiced = 5 '7
        Opened = 6 '8
        PartiallyInvoiced = 7 '9
    End Enum

    Public Enum CRStatus
        newCR = 1
        Acknowledged = 2
    End Enum

    Public Enum POStatus_new 'old status
        'Michelle (15/11/2010) - new status : 0
        Draft = 0
        'null = 0
        NewPO = 1 '1
        Open = 2 '8
        Accepted = 3 '2
        Rejected = 4 '3
        Cancelled = 5 '4
        Close = 6
        Submitted = 7
        PendingApproval = 8
        Approved = 9
        RejectedBy = 10
        'CancelledBy = 11
        HeldBy = 11
        Void = 12
        CancelledBy = 13
    End Enum

    Public Enum IRStatus_new
        Submitted = 1
        Approved = 2
        PendingApproval = 3
        Rejected = 4
    End Enum

    Public Enum MRSStatus_new
        NewMRS = 1
        Issued = 2
        Acknowledged = 3
        AutoAcknowledged = 4
        Cancelled = 5
        Rejected = 6
        PartialIssued = 7
    End Enum

    Public Enum IQCStatus_new
        Approved = 1
        Waived = 2
        Replacement = 3
        Rejected = 4
    End Enum

    Public Enum DOStatus
        Draft = 1
        Submitted = 2
        FullyAccepted = 3
        PartiallyAccepted = 4
        Rejected = 5
        Invoiced = 6
    End Enum

    Public Enum Fulfilment
        null = 0
        Open = 1
        Part_Delivered = 2
        Fully_Delivered = 3
        'Completed = 4
        Pending_Cancel_Ack = 4
        Cancel_Order = 5
    End Enum

    Public Enum GRNStatus
        Uninvoice = 1
        Invoiced = 2
        PendingACK = 3
    End Enum

    Public Enum CatalogueStatus
        Draft = 1
        'PMPending = 2
        'PMRejected = 3
        'VendorAck = 4
        'HubPending = 5
        'HubRejected = 6
        'HubApproved = 7
        'PendingDeactivated = 8
        'Deactivated = 9
        BuyerPending = 2
        BuyerRejected = 3
        HubPending = 4
        Rejected = 5
        Approved = 6
    End Enum

    Public Enum ListPriceStatus
        PendingApproval = 1
        Rejected = 2
        Approved = 3
    End Enum

    Public Enum WheelDateFormat
        LongDateDay '//Wednesday, 07 Nov 2004
        LongDate    '//07 Nov 2004
        ShortDate   '// 07/11/2004
        LongDateTimeDay '//Wednesday, 07 Nov 2004 11:40:39
        LongDateTime '//07 Nov 2004 11:40:39
        CountDownDate '//june 16, 2005 18:00:00
        Time '//HH:mm:ss
    End Enum

    Public Enum WheelMailPriority
        High = 1
        Low = 2
        Normal = 3
    End Enum

    Public Enum WheelMsgNum
        Save = 1
        NotSave = 2
        Delete = 3
        NotDelete = 4
        Duplicate = 5
    End Enum

    Public Enum ExceptionType
        System = 0
        Input = 1
        PRErr = 2
        POErr = 3
        DOErr = 4
    End Enum

    Public Enum CObjectType
        TextBox = 1
        DropDownList = 2
        CheckBox = 3
        RadioButton = 4
    End Enum

    Public Enum WheelModule
        SecurityControl
        PRMod
        OrderMgnt
        Fulfillment
        CompanyProfile
        UserProfile
        Admin
        CatalogueMgnt
        RFQ
        InvoiceMod
        IQCMod
        IRMod
        MRSMod
        ROMod
        WOMod
        RIMod
        'Chee Hong 2015.02.02 Agora Stage 2
        DebitNoteMod
        CreditNoteMod
        'Chee Hong 2015.11.16 Staff Claim Module
        StaffClaimMod
    End Enum

    Public Enum EmailType
        RFQRequested
        RFQRejected
        RFQReply
        BudgetAdvisory
        BudgetTopUp
        PRConsolidated
        PRCancelled
        PRHeld
        PRRejected
        POCreated
        POApproved
        PORaised
        PORejected
        PORejectedBy
        POAccepted
        DOCreated
        DOCreatedToSK
        GoodsReceiptNoteCreated
        GoodsReceiptNoteReject
        AckGRN
        InvoiceCreated
        InvoiceApproval
        POCancellationRequest
        POCancellationRequestToAOBuyer
        POCancellationRequestToAO
        AckPOCancellationRequest
        CatalogueApproval
        CatalogueRejected
        CatalogueDiscarded
        CataloguePublished
        ListPriceUpdate
        ListPriceApproval
        ListPristRejected
        ListPricePublished
        ChangePwd
        NewUserAccount
        CompanyActivated
        CompanyDeactivated
        RFPPublishApp
        SendRFPInviClosed
        RFPInviAccepted
        RFPInviRejected
        SendMaintDocApp
        SendMaintDocAppExt
        SendMaintDoc
        SendMaintDocExt
        MaintDocReadAck
        RFPAwardApp
        SendRFPAward
        VPublicRFPReg
        VPublicRFPRegRej
        ApprovedPublicRFPReg
        NewVCoyReg
        ApprovedVCoyReg
        VQuery
        RejectedVCoyReg
        ResponseQuery
        ReliefStaffAssign
        ExtendReliefStaffAssign
        TechUpload
        FinUpload
        VComplianceList
        VDrawing
        WitnesingOpenTechG
        WitnesingOpenFinG
        NewRegistPublicVendor
        RejectRFPReg
        HoldRFPReg
        sendMainPublish
        sendAwardPublish
        RFQSupply
        POHeld
        PRCancelledToBuyer
        IQCApprovedToSK
        IQCRejectedToSK
        RIAcknowledged
        RIRejected
        IRApprovedToHOD
        IRRejectedToRequestor
        IRToSK
        MRSToSK
        MRSIssued
        MRSRejected
        ROCreated
        RICreated
        'Jules 2015.02.02 Agora Stage 2
        FOIncomingDN
        FMIncomingDN
        FMIncomingCN
        'CH - 2016/02/24 - Staff Claim
        SCApproved
        SCRejected
    End Enum

    Public Enum WheelUserActivity
        Login
        Logout
        B_SubmitPR
        B_CancelPR
        B_DeletePR
        AO_ApprovePR
        AO_RejectPR
        AO_HoldPR
        AO_CreatePO
        B_SubmitRFQ
        V_AcceptPO
        V_RejectPO
        V_SaveDO
        V_SubmitDO
        V_SubmitQuotation
        B_GRN
        B_GRNACK
        V_Invoice
        BA_ModCoy
        BA_ModDept
        BA_ModCoyParam

        FO_ApproveInvoice
        FO_RejectInvoice
        FO_HoldInvoice
        FM_ApprovePayment
        FM_HoldPayment
        B_SubmitPO
        B_CancelPO
        B_DeletePO
        AO_ApprovePO
        AO_RejectPO
        AO_HoldPO
        B_VoidPO
        AO_ApproveIQC
        AO_WaiveIQC
        AO_ReplaceIQC
        AO_ReTestIQC
        AO_RejectIQC
        AO_ApproveIR
        AO_RejectIR
        SK_ApproveMRS
        SK_RejectMRS
        REQ_SubmitIR
        REQ_AckMRS
        REQ_CancelMRS
        SK_SubmitRO
        SK_SubmitWO
        SK_AckRI
        SK_RejectRI
        REQ_SubmitRI
        'Jules 2015.02.02 Agora Stage 2
        V_DebitNote
        V_CreditNote
        'Chee Hong 2015.02.02 Agora Stage 2
        FO_VerifyDN
        FM_ApproveDN
        FM_AckCN
        'Chee Hong 2015.11.16 Staff Claim Module
        Staff_SaveHardship
        Staff_SaveOvertime
        Staff_SaveAllowance
        Staff_SaveEntertain
        Staff_SaveTransport
        Staff_SaveMisc
        Staff_SaveOutstation
        Staff_SubmitStaffClaim
        AO_ApproveSC
        AO_RejectSC
    End Enum

    Public Enum EnumRFPStatus
        Draft = 1
        Pending = 2
        Sent = 3
        Approved = 4
        Held = 5
        Rejected = 6
        Closed = 7
        Withdrawn = 8
    End Enum
    Public Enum DocMAINT
        Addm
        Corrm
        Clarif
        ExtDate
        Withd
        Query
        RFP
        AWARD
    End Enum

    Public Enum MAINT
        Clarification = 2
        Query = 5
    End Enum

    Public Enum VendorRegApprStatus
        Pending = 0
        Approved = 1
        Reject = 2
    End Enum

    Public Enum EnumInterestStatus
        Interest = 0
        NotInterest = 1

    End Enum

    Public Enum EnumRFPV
        responded = 1
        draft = 2
        read = 3
        accepted = 4
        rejected = 5
        pending = 6
    End Enum

    Public Enum EnumViewAward
        viewableAll = 0
        viewableResonly = 1
        viewableAward = 2

    End Enum

    Public Enum EnumRFPTechShort
        shortlisted = 1
    End Enum

    Public Enum EnumRFPFOTUpdate
        SendFOTUpdate = 0
        FOTUpdated = 1
    End Enum

    Public Enum EnumEarnestMon
        Required = 0
        NotRequired = 1
    End Enum

    Public Enum EnumRFPINVI
        Responded = 1
        NoResponse = 2
    End Enum

    Public Enum EnumRFPPayment
        Pending = 7
        Paid = 8
    End Enum

    Public Enum EnumRFPFOT
        Contract = 0
        Catalogue = 1
    End Enum

    Public Enum EnumFOTCon
        Rate = 1
        Quantity = 2
    End Enum

    Public Enum EnumRFPQuery
        draft = 1
        sent = 2
        replied = 3
    End Enum

    Public Enum EnumRFPResponse
        draft = 1
        sent = 2
    End Enum

    Public Enum DocUpload
        COMLIST
        FINDOC
        RELATEDDOC
        RFPDR
        TECDOC
        RFPDOC
        CONFORM
        ACCEPTFORM
        AGREEFORM
        GUAFORM
    End Enum

    Public Enum PM
        BD
        Cheque
    End Enum

    Public Enum TypeStatus
        Grant = 0
        NotGranted = 1
    End Enum
    Public Enum EmailStatus
        EmailNotSent = 0
        EmailSent = 1
    End Enum

    Public Enum EnumRFPApproach
        OneEnvelop
        TwoEnvelop
    End Enum

    Public Enum EnumRFPProcurement
        open = 0
        close = 1
    End Enum
    Public Enum EnumDOCOWNERTYPE
        RFPDOC
        COMLIST
        TECDOC
        FINDOC
        RFPDR
        RELATEDDOC
    End Enum

    Public Enum EnumAppPackage
        eProcure
        eRFP
        eAuc
        eCon
        All
    End Enum

    Public Structure UploadProduct
        Dim DBFiels As String
        Dim AllowNull As String
    End Structure

    Public Enum RIStatus
        Submitted = 1
        Acknowledged = 2
        Rejected = 3
    End Enum

    Public Enum WOStatus
        Submitted = 1
        PendingApproval = 2
        Approved = 3
        Cancelled = 4
        Rejected = 5
    End Enum

    Public Class Common

        '************* DECLARE ENUMERATIONS ****************
        'Module modApp
        '//For Messaging
        Public Const RecordSave As String = "Record saved."
        Public Const RecordDelete As String = "Record deleted."
        Public Const RecordNotDelete As String = "Record not deleted."
        Public Const RecordDuplicate As String = "Duplicate record found."
        Public Const RecordNotSave As String = "Record not saved."
        '//for duplication of transaction like PR,PO,DO and etc.
        Public Const MsgTransDup As String = "Duplicate transaction number found.""&vbCrLf&""Please contact your Administrator to rectify the problem."

        Public Const RecordUsrGrpCascade As String = "Deletion is not allow. ""&vbCrLf&""This user group is active and being used."
        Public Const RecordTiedTxn As String = "Deletion is not allow. ""&vbCrLf&""User has either created transaction or he/she is tied to an approval group." 'Record tied to txn

        'Public Const UserLimit As String = "The number of active users exceeds the User License."      'user limit reached
        Public Const RecordUserCascade As String = "Deletion not allow. ""&vbCrLf&""Due to record have active users."      'Record tie to user
        Public Const RecordVendorCascade As String = "Deletion not allow. ""&vbCrLf&""Due to record is a approved vendor." 'Record is a approved vendor


        Public Const RecordUsed As String = "This ID has been taken previously. ""&vbCrLf&""Please enter another ID."  '
        Public Const MSGBFDELETE As String = "Are you sure you want to delete this record?"

        Public Const RecordDeleteNotAllowed As String = "Deletion is not allowed.""&vbCrLf&""It has outstanding PR(s)."
        '//System Error
        Public ERRNUM As String
        Public Const EmailCompGen As String = "Note: This is a system generated email. Please do not reply to this email."
        Public EmailFooter As String = ConfigurationSettings.AppSettings("EmailFooter")   '"Yours sincerely, <BR>Hub Administrator<BR>TX123 (M) Sdn Bhd<BR>Phone: 03-7954 7311<BR>Fax:     03-7954 7322<BR>Website: www.tx123.com.my<P>"
        Public EmailHomeAddr As String = ConfigurationSettings.AppSettings("CompanyWeb") '"<A href='http://www.tx123.com.my'>http://www.tx123.com.my</A> "
        Public EmailHomeEhubAddr As String = ConfigurationSettings.AppSettings("EhubWeb")  '"<A href='https://www.tx123.com.my/tx123/ehub/'>https://www.tx123.com.my/tx123/ehub/</A> "
        Public EmailHomeeRFPAddr As String = ConfigurationSettings.AppSettings("eRFPWeb")  '"<A href='https://www.tx123.com.my/tx123/ehub/'>https://www.tx123.com.my/tx123/ehub/</A> "

        Public EmailFooterENT As String = ConfigurationSettings.AppSettings("EmailFooterENT")   '"Yours sincerely, <BR>Hub Administrator<BR>TX123 (M) Sdn Bhd<BR>Phone: 03-7954 7311<BR>Fax:     03-7954 7322<BR>Website: www.tx123.com.my<P>"
        Public EmailHomeENTAddr As String = ConfigurationSettings.AppSettings("CompanyWebENT") '"<A href='http://www.tx123.com.my'>http://www.tx123.com.my</A> "
        Public EmailHomeEhubENTAddr As String = ConfigurationSettings.AppSettings("EhubWebENT")  '"<A href='https://www.tx123.com.my/tx123/ehub/'>https://www.tx123.com.my/tx123/ehub/</A> "
        Public EmailHomeeRFPENTAddr As String = ConfigurationSettings.AppSettings("eRFPWebENT")  '"<A href='https://www.tx123.com.my/tx123/ehub/'>https://www.tx123.com.my/tx123/ehub/</A> "

        'Public Const EmailHome As String = "Wheel"

        'End Module


        'Name       : SelDdl
        'Author     : Kaithim
        'Descption  : Select Dropdownlist with Value  
        'Remark     :
        'ReturnValue: 
        'LastUpadte : 19 Sep 2002
        'Version    : 1.00
        Public Shared Function SelDdl(ByVal pInVal As Object,
                                ByRef pDropDownList As UI.WebControls.DropDownList,
                                Optional ByVal pValueField As Boolean = True,
                                Optional ByVal pUcase As Boolean = False) As Boolean
            Try
                SelDdl = False
                Dim lngLoop As Long
                pDropDownList.SelectedItem.Selected = False
                If Not IsDBNull(pInVal) Then
                    'Dim ary() As String = Split(pInString, ",")
                    Dim varItem
                    For Each varItem In pDropDownList.Items
                        If pValueField Then
                            If varItem.value = pInVal Then
                                varItem.selected = True
                                SelDdl = True
                                Exit For
                            End If
                        Else
                            If pUcase Then
                                If UCase(varItem.Text) = UCase(pInVal) Then
                                    varItem.selected = True
                                    Exit For
                                    SelDdl = True
                                End If
                            Else
                                If varItem.Text = pInVal Then
                                    varItem.selected = True
                                    SelDdl = True
                                    Exit For
                                End If
                            End If
                        End If

                    Next
                Else
                    pDropDownList.SelectedIndex = 0
                End If
            Catch exp As Exception
                Return False
                pDropDownList.SelectedIndex = 0
            End Try
        End Function

        'Added by Joon on 23rd April 2012
        Public Shared Function SelDdl2(ByVal pInVal As Object,
                                ByRef pDropDownList As UI.WebControls.DropDownList,
                                Optional ByVal pValueField As Boolean = True,
                                Optional ByVal pUcase As Boolean = False) As Boolean
            Try
                SelDdl2 = False
                Dim lngLoop As Long
                pDropDownList.SelectedItem.Selected = False
                If Not IsDBNull(pInVal) Then
                    'Dim ary() As String = Split(pInString, ",")
                    Dim varItem
                    For Each varItem In pDropDownList.Items
                        If pValueField Then
                            If UCase(varItem.value) = UCase(pInVal) Then
                                varItem.selected = True
                                SelDdl2 = True
                                Exit For
                            End If
                        Else
                            If pUcase Then
                                If UCase(varItem.Text) = UCase(pInVal) Then
                                    varItem.selected = True
                                    Exit For
                                    SelDdl2 = True
                                End If
                            Else
                                If varItem.Text = pInVal Then
                                    varItem.selected = True
                                    SelDdl2 = True
                                    Exit For
                                End If
                            End If
                        End If

                    Next
                Else
                    pDropDownList.SelectedIndex = 0
                End If
            Catch exp As Exception
                Return False
                pDropDownList.SelectedIndex = 0
            End Try
        End Function

        'Name       : Parse
        'Author     : Kaithim
        'Descption  : replace a html text to SQL text.
        'Remark     :
        'ReturnValue: 
        'LastUpadte : 01 Oct 2002
        'Version    : 1.00
        Public Shared Function Parse(ByVal pInstring As String) As String
            pInstring = Replace(pInstring, "'", "''")
            Return pInstring
        End Function

        'Name       : FillDdl
        'Author     : Kaithim
        'Descption  : fill dropdownlist with a source 
        'Remark     :
        'ReturnValue: 
        'LastUpadte : 01 Oct 2002
        'Version    : 1.00
        Public Shared Function FillDdl(ByRef pDropDownList As UI.WebControls.DropDownList,
                                ByVal pstrText As String,
                                ByVal pstrValue As String,
                                ByRef pDataSource As Object,
                                Optional ByVal pDefaultText As String = "") As Boolean

            pDropDownList.DataSource = pDataSource
            pDropDownList.DataTextField = pstrText
            pDropDownList.DataValueField = pstrValue
            pDropDownList.DataBind()

            'kk.remark.24/11/2004.Optional to add a text in the default ddl
            If pDefaultText <> "" Then
                pDropDownList.Items.Insert(0, pDefaultText)
            End If
        End Function

        'Public Shared Function FillDdl_(ByRef pDropDownList As DataView,
        '                        ByVal pstrText As String,
        '                        ByVal pstrValue As String,
        '                        ByRef pDataSource As Object,
        '                        Optional ByVal pDefaultText As String = "") As Boolean

        '    pDropDownList.DataSource = pDataSource
        '    pDropDownList.DataTextField = pstrText
        '    pDropDownList.DataValueField = pstrValue
        '    pDropDownList.DataBind()

        '    'kk.remark.24/11/2004.Optional to add a text in the default ddl
        '    If pDefaultText <> "" Then
        '        pDropDownList.Items.Insert(0, pDefaultText)
        '    End If
        'End Function

        'Name       : FillDefault
        'Author     : kk
        'Descption  : fill dropdownlist with all record in a table
        'Remark     : Use to bind like company and dept cbo
        'ReturnValue: 
        'LastUpadte : 25 Nov 2004
        'Version    : 1.00
        Public Shared Function FillDefault(ByRef pDropDownList As UI.WebControls.DropDownList,
                                ByVal pstrTable As String,
                                ByVal pstrText As String,
                                ByVal pstrValue As String,
                                Optional ByVal pDefaultText As String = "",
                                Optional ByVal pWhere As String = "") As Boolean

            Dim strSQL As String
            Dim objDb As New EAD.DBCom

            strSQL = "SELECT * FROM " & pstrTable
            If pWhere <> "" Then
                strSQL &= " WHERE " & pWhere
            End If
            strSQL &= " ORDER BY " & pstrText

            pDropDownList.DataSource = objDb.GetView(strSQL)
            pDropDownList.DataTextField = pstrText
            pDropDownList.DataValueField = pstrValue
            pDropDownList.DataBind()

            'kk.remark.24/11/2004.Optional to add a text in the default ddl
            If pDefaultText <> "" Then
                Dim ls As New ListItem
                ls.Text = pDefaultText
                If pDefaultText = "---Select---" Then
                    ls.Value = ""
                End If
                pDropDownList.Items.Insert(0, ls)

            End If
        End Function




        'Name       : FillDefault
        'Author     : kk
        'Descption  : fill dropdownlist with all record in a table
        'Remark     : Use to bind like company and dept cbo
        'ReturnValue: 
        'LastUpadte : 25 Nov 2004
        'Version    : 1.00
        Public Shared Function FillDefault(ByRef pList As UI.WebControls.ListBox,
                                ByVal pstrTable As String,
                                ByVal pstrText As String,
                                ByVal pstrValue As String,
                                Optional ByVal pWhere As String = "") As Boolean

            Dim strSQL As String
            Dim objDb As New EAD.DBCom

            strSQL = "SELECT * FROM " & pstrTable
            If pWhere <> "" Then
                strSQL &= " WHERE " & pWhere
            End If
            strSQL &= " ORDER BY " & pstrText


            pList.DataSource = objDb.GetView(strSQL)
            pList.DataTextField = pstrText
            pList.DataValueField = pstrValue
            pList.DataBind()

        End Function

        'Name       : GetIndex
        'Author     : Kk
        'Descption  : Get the Index no from DropDownList by Value 
        'Remark     : 
        'ReturnValue: Integer
        'LastUpadte : 13 Oct 2002
        'Version    : 1.00
        Public Shared Function FillLst(ByRef pList As ListBox,
                                ByVal pstrText As String,
                                ByVal pstrValue As String,
                                ByRef pDataSource As Object) As Boolean

            pList.DataSource = pDataSource
            pList.DataTextField = pstrText
            pList.DataValueField = pstrValue
            pList.DataBind()
        End Function

        'Name       : SortListControl
        'Author     : Kk
        'Descption  : Sort item in the object by moving text and value
        'Remark     :
        'ReturnValue: 
        'LastUpadte : 18 Dec 2004
        'Version    : 1.00
        Public Shared Sub SortListControl(ByRef SourceList As ListControl, ByVal Ascending As Boolean)
            'sorts listcontrols
            Dim array1 As New ArrayList
            Dim loop1 As Integer
            For loop1 = 0 To SourceList.Items.Count - 1
                array1.Add(SourceList.Items(loop1))
            Next

            Dim myComparer = New SortListArray(Ascending)
            array1.Sort(myComparer)

            SourceList.Items.Clear()
            For loop1 = 0 To array1.Count - 1
                SourceList.Items.Add(array1(loop1))
            Next
        End Sub

#Region " New Class for IComparer use by SortListControl"
        Public Class SortListArray
            Implements IComparer
            Private _Ascending As Boolean = True
            Public Sub New()
            End Sub
            Public Sub New(ByVal Ascending As Boolean)
                _Ascending = Ascending
            End Sub
            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
                If _Ascending Then
                    Return New CaseInsensitiveComparer().Compare(x.ToString, y.ToString)
                Else
                    Return New CaseInsensitiveComparer().Compare(y.ToString, x.ToString)
                End If
            End Function
        End Class
#End Region

        'Name       : Sort Object(List,Combo) Item
        'Author     : Kk
        'Descption  : Sort item in the object
        'Remark     :
        'ReturnValue: 
        'LastUpadte : 11 Jun 2004
        'Version    : 1.00
        Public Shared Function SortLst(ByRef pList As Object) As Boolean
            If pList.Items.Count > 0 Then
                Dim ary(pList.Items.Count - 1) As String
                Dim i As Integer

                For i = 0 To pList.Items.Count - 1
                    ary(i) = pList.Items(i).text
                Next

                ary.Sort(ary)
                pList.Items.Clear()

                For i = 0 To UBound(ary)
                    pList.Items.Add(ary(i))
                Next
            End If
        End Function

        'Author     : kai thim
        'Descption  : Message Box
        'Remark     :
        'ReturnValue: 
        'LastUpadte : 17 Dec 2002
        'Version    : 1.00
        Public Shared Sub NetMsgbox(ByVal pg As System.Web.UI.Page, ByVal msg As String, Optional ByVal style As MsgBoxStyle = MsgBoxStyle.Exclamation, Optional ByVal title As String = "Procurement")

            Dim vbs As String
            vbs = vbs & "<script language=""vbs"">"
            vbs = vbs & "Call MsgBox(""" & msg & """, " & style & ", """ & title & """)"
            'vbs = vbs & "Call MsgBox(""" & msg & """, " & style & ","")"
            vbs = vbs & "</script>"
            Dim rndKey As New Random
            pg.RegisterStartupScript(rndKey.Next.ToString, vbs)
        End Sub

        Public Shared Sub NetMsgbox(ByVal pg As System.Web.UI.Page, ByVal msg As String, ByVal pRedirect As String, Optional ByVal style As MsgBoxStyle = MsgBoxStyle.Exclamation, Optional ByVal title As String = "Procurement")
            Dim vbs As String
            vbs = vbs & "<script language=""vbs"">"
            vbs = vbs & "Call MsgBox(""" & msg & """, " & style & ", """ & title & """)"
            vbs = vbs & vbLf & "window.location=""" & pRedirect & """"
            vbs = vbs & "</script>"
            Dim rndKey As New Random
            pg.RegisterStartupScript(rndKey.Next.ToString, vbs)
        End Sub

        Public Shared Sub NetMsgboxTest(ByVal pg As System.Web.UI.Page, ByVal msg As String, ByVal pRedirect As String, Optional ByVal style As MsgBoxStyle = MsgBoxStyle.Exclamation, Optional ByVal title As String = "Procurement")

            'Dim rndKey As New Random
            'pg.RegisterStartupScript(rndKey.Next.ToString, "<script>errBody = '" & msg & "'; errTitle = 'Message Box Title';</script>")
            Dim vbs As String
            vbs = vbs & "<script language=""javascript"">"
            vbs = vbs & "Call MsgBox(""" & msg & """, " & style & ", """ & title & """)"
            vbs = vbs & vbLf & "window.location=""" & pRedirect & """"
            vbs = vbs & "</script>"
            Dim rndKey As New Random
            pg.RegisterStartupScript(rndKey.Next.ToString, vbs)

        End Sub

        Public Shared Sub NetPrompt(ByVal pg As System.Web.UI.Page, ByVal msg As String,
        ByVal pRedirect As String, Optional ByVal title As String = "Procurement", Optional ByVal pOtherRedirect As String = "")
            Dim vbs As String
            vbs = vbs & "<script language=""vbs"">"
            vbs = vbs & "result = MsgBox(""" & msg & """, " & MsgBoxStyle.YesNo & ", """ & title & """)"
            vbs = vbs & vbLf & "if result=vbYes Then"
            vbs = vbs & vbLf & "window.location=""" & pRedirect & """"

            If pOtherRedirect <> "" Then
                vbs = vbs & vbLf & "else"
                vbs = vbs & vbLf & "window.location=""" & pOtherRedirect & """"
            End If

            vbs = vbs & vbLf & "end if"
            vbs = vbs & "</script>"
            Dim rndKey As New Random
            pg.RegisterStartupScript(rndKey.Next.ToString, vbs)
        End Sub


        Public Shared Function parseNull(ByVal pIn As Object, Optional ByVal pRplc As Object = "") As Object
            If IsDBNull(pIn) Then
                Return pRplc
            Else
                Return pIn
            End If
        End Function

        'keep a growth one dimension array
        Public Shared Sub Insert2Ary(ByRef pQuerys() As String,
                           ByVal pstrSQL As String)

            If UBound(pQuerys) = 0 And pQuerys(0) = vbNullString Then
                pQuerys(0) = pstrSQL
            Else
                ReDim Preserve pQuerys(UBound(pQuerys) + 1)
                pQuerys(UBound(pQuerys)) = pstrSQL
            End If
        End Sub

        Public Shared Function TrwExp(ByVal pExp As Exception, Optional ByVal pIn As String = "", Optional ByVal pExpType As Integer = 0) As Exception
            Dim strMessage, strMessageTemp As String
            Dim WriteStr As New Text.StringBuilder
            Dim fixRand As New Random
            Dim intRnd As Int32
            Dim ctx As HttpContext = HttpContext.Current
            Dim intPos As Int32
            Dim dDispatcher As New AgoraNEO.dispatcher
            'If pIn = "" Then
            '    strMessage = pExp.Message
            'Else
            '    strMessage = "[" & pIn.ToString & "]--" & pExp.Message
            'End If

            '//Make to upper case for easy comparision
            strMessageTemp = pExp.Message.ToUpper
            '//No to show Database Field Name and Database Table Name at Browser,
            '//Only save to Database.
            If InStr(strMessageTemp, "[UPDATE") > 0 _
            Or InStr(strMessageTemp, "[INSERT") > 0 _
            Or InStr(strMessageTemp, "[SELECT") > 0 _
            Or InStr(strMessageTemp, "[SHAPE") > 0 _
            Or InStr(strMessageTemp, "[DELETE") > 0 Then
                intPos = InStr(pExp.Message, "--")
                strMessage = pExp.Message.Substring(intPos + 1)
            ElseIf InStr(strMessageTemp, "VIOLATION OF PRIMARY KEY") > 0 Then
                intPos = InStr(pExp.Message, "VIOLATION OF PRIMARY KEY")
                strMessage = pExp.Message.Substring(1, intPos - 1)
            Else
                strMessage = pExp.Message
            End If

            '//?????? what is this 17/10/2005 by Moo
            If pExp.GetType Is GetType(CustomException) Then
                ctx.Session("ErrMsg") = strMessage
                ctx.Server.ClearError()
                ctx.Response.Redirect(dDispatcher.direct("Initial", "ErrorPage.aspx"), False)
                Exit Function
            End If

            'pExp.GetBaseException.GetObjectData()
            'pexp.InnerException
            'Dim expCustomise As New Exception(strMessage)

            'ctx.Session("UserID") = "moo"
            'ctx.Session("CompanyID") = "kompakar"
            Try
                intRnd = fixRand.Next()
                Dim strErrId As String = ctx.Session.SessionID & "--" & Convert.ToString(intRnd)

                Dim WriteStr1 As New Text.StringBuilder
                'WriteStr1.Append("<B>User ID : </B>" & ctx.Session("UserID"))
                WriteStr1.Append("<B>Error ID : </B>")
                WriteStr1.Append(strErrId).Append("<BR>")
                WriteStr1.Append("<B>Message : </B>")
                WriteStr1.Append(strMessage).Append("<BR>")
                WriteStr1.Append("<B>Error Source : </B>")
                WriteStr1.Append(pExp.Source).Append("<BR>")
                WriteStr1.Append("<B>Error Stack Trace : </B>").Append("<BR>")
                WriteStr1.Append(pExp.StackTrace).Append("<BR>")
                ctx.Session("ErrMsg") = WriteStr1.ToString

                If pExp.GetType() Is GetType(InputException) Then
                    ctx.Session("back") = "show"
                End If
                If Convert.ToBoolean(ConfigurationSettings.AppSettings("LogError")) Then
                    If ConfigurationSettings.AppSettings("LogMethod").ToUpper = "TEXTFILE" Then
                        WriteStr.Append(StrDup(150, "*")).Append(vbCrLf)
                        WriteStr.Append("Error ID : ")
                        WriteStr.Append(strErrId).Append(vbCrLf)
                        WriteStr.Append("Date : ")
                        WriteStr.Append(Now).Append(vbCrLf)
                        WriteStr.Append("Current User : ")
                        WriteStr.Append(ctx.Session("UserID")).Append("(")
                        WriteStr.Append(ctx.Session("CompanyID")).Append(")").Append(vbCrLf)
                        WriteStr.Append("Message : ")
                        WriteStr.Append(strMessage).Append(vbCrLf)
                        WriteStr.Append("Error Source : ")
                        WriteStr.Append(pExp.Source).Append(vbCrLf)
                        WriteStr.Append("Error Stack Trace : ").Append(vbCrLf)
                        WriteStr.Append(pExp.StackTrace).Append(vbCrLf)
                        WriteStr.Append("Error Target Site : ")
                        WriteStr.Append(pExp.TargetSite).Append(vbCrLf)
                        WriteStr.Append(StrDup(150, "*")).Append(vbCrLf)
                        WriteLog(WriteStr.ToString)
                    ElseIf ConfigurationSettings.AppSettings("LogMethod").ToUpper = "DATABASE" Then
                        'LogToDB(pExp)
                        Dim myData As New AppDataProvider
                        myData.LogToDB(pExp, strErrId, pIn)
                        myData = Nothing
                    End If
                End If
                'writestr.Remove
                ctx.Server.ClearError()
                'ctx.Response.Redirect(ctx.Request.ApplicationPath & "/" & ConfigurationSettings.AppSettings("ErrorPage"), False)
                ctx.Response.Redirect(dDispatcher.direct("Initial", "ErrorPage.aspx"), False)
                'Current.Server.Transfer("ErrorPage.aspx", False)
                '******************************************************************************
                'Error When using Server.Transfer --> "Error executing child request"
                'Error When Using Response.Redirect(page,true) --> "Thread was being aborted."
                '******************************************************************************
            Catch e1 As Exception
                EventLog.WriteEntry(e1.Source, "Error Occured in Function TrwExp--" & e1.Message, EventLogEntryType.Error, 65535)
            Finally
                'myData = Nothing
                ctx.Server.ClearError()
            End Try
            'Throw expCustomise
        End Function

        Public Shared Function ConvertDate(ByVal date_day) As String
            Dim str As String
            If IsDate(date_day) Then
                'str = "Convert(DateTime,'" & Common.Parse(date_day) & "',103)"
                str = "'" & Format(CDate(date_day), "yyyy-MM-dd HH:mm:ss") & "'"
            Else
                str = "NULL"
            End If

            ConvertDate = str

        End Function

        Public Shared Function ConvertMoney(ByVal sMoney As String) As String
            Dim str As String
            'Michelle (20/9/2010) - To return the orignal value as MY SQL is without 'Money'
            'str = "Convert(Money,'" & Common.Parse(sMoney) & "',103)"
            str = Common.Parse(sMoney)

            ConvertMoney = str

        End Function

        Shared Function FormatWheelDate(ByVal WheelDateFormat As WheelDateFormat, ByVal objDate As Object) As String
            If IsDate(parseNull(objDate)) Then
                Dim dteDate As Date
                dteDate = Convert.ToDateTime(objDate)
                Select Case WheelDateFormat
                    Case WheelDateFormat.LongDateDay
                        FormatWheelDate = Format(dteDate, "ddd, dd MMM yyyy")
                    Case WheelDateFormat.LongDate
                        '//modify to apply to all
                        FormatWheelDate = Format(dteDate, "d") 'Format(dteDate, "dd MMM yyyy")
                    Case WheelDateFormat.ShortDate
                        FormatWheelDate = Format(dteDate, "d") 'Format(dteDate, "dd/MM/yyyy")
                    Case WheelDateFormat.LongDateTimeDay
                        FormatWheelDate = Format(dteDate, "ddd, dd MMM yyyy HH:mm:ss")
                    Case WheelDateFormat.LongDateTime
                        FormatWheelDate = Format(dteDate, "dd MMM yyyy HH:mm:ss")
                    Case WheelDateFormat.CountDownDate
                        FormatWheelDate = Format(dteDate, "MMM d, yyyy HH:mm:ss")
                    Case WheelDateFormat.Time
                        FormatWheelDate = Format(dteDate, "HH:mm")
                End Select
            Else
                '//HOW
            End If
        End Function

        Public Shared Function WriteLog(ByVal pMsg As String) As Boolean
            Dim ctx As HttpContext = HttpContext.Current
            WriteLog = True
            Dim objStreamWriter As StreamWriter
            Try
                objStreamWriter = New StreamWriter(ctx.Server.MapPath(ctx.Request.ApplicationPath & "\" & Format(Now.Date, "yyyyMMdd") & "_Error.log"), True)
                objStreamWriter.WriteLine(pMsg)

            Catch Meexp As Exception
                WriteLog = False
            Finally
                objStreamWriter.Close()
                objStreamWriter = Nothing
            End Try
        End Function

        Public Shared Function ParseSQL(ByVal pInString As String) As String
            Dim strTemp As String
            If InStr(pInString, "*") > 0 Then
                strTemp = " LIKE '" & Parse(Replace(pInString, "*", "%")) & "'"
            ElseIf InStr(pInString, "?") > 0 Then
                strTemp = " LIKE '" & Parse(Replace(pInString, "?", "_")) & "'"
            Else
                strTemp = " = '" & Parse(pInString) & "'"
            End If
            Return strTemp
        End Function
        Public Shared Function BuildWildCard(ByVal VenName As String) As String
            Dim strTemp As String
            If InStr(VenName, "*") <= 0 And InStr(VenName, "?") <= 0 Then
                strTemp = "*" & VenName & "*"
            Else
                strTemp = VenName
            End If
            Return strTemp
        End Function

        'Author     : Ai Chu
        'Descption  : check remarks maxlength
        'Remark     :
        'ReturnValue: 
        'LastUpadte : 15 Feb 2005
        'Version    : 1.00
        Public Shared Function checkMaxLength(ByVal pStr As String, ByVal pCnt As Integer) As Boolean
            If pStr.Length > pCnt Then
                checkMaxLength = False
            Else
                checkMaxLength = True
            End If
        End Function

        Public Shared Function isImageFile(ByVal pStrFile As String) As Boolean
            Dim strFileType As String
            If Not IsNothing(pStrFile) Then
                strFileType = pStrFile.Substring(pStrFile.IndexOf(".") + 1)

                If strFileType.ToLower = "gif" Or strFileType.ToLower = "jpg" Or strFileType.ToLower = "jpeg" Then 'Or strFileType.ToLower = "tif" Then
                    isImageFile = True
                Else
                    isImageFile = False
                End If

            Else
                isImageFile = False
            End If

        End Function

        'Name       : encrypt file 
        'Author     : Gary
        'Descption  : encrypt file 
        'Remark     :
        'ReturnValue: 
        'LastUpadte : 5 july 2005
        'Version    : 1.00

        Public Shared Function encryption(ByVal inFile As String, ByVal outFile As String)
            Dim objenc As New CryptoMe
            Dim inFile1 As String = inFile
            Dim outFile1 As String = outFile + ".eRFP"
            Dim password As String = "gary"
            objenc.EncryptFile(inFile1, outFile1, password)
        End Function


        'Name       : Decrypt file 
        'Author     : Gary
        'Descption  : Decrypt file 
        'Remark     :
        'ReturnValue: 
        'LastUpadte : 5 july 2005
        'Version    : 1.00

        Public Shared Function Decryption(ByVal inFile As String, ByRef outFile As String)
            Dim objenc As New CryptoMe
            Dim inFile1 As String = inFile
            'Dim index As Integer = inFile.LastIndexOf(".eRFP")
            'outFile = inFile.Substring(0, index)
            outFile = System.IO.Path.GetTempFileName()
            Dim password As String = "gary"
            objenc.DecryptFile(inFile1, outFile, password)
        End Function

        Public Shared Function EncryptString(ByVal pValue As String, Optional ByVal pKey As String = "[+-*/]") As String
            Dim UE As New UnicodeEncoding
            Dim KeyBytes() As Byte = UE.GetBytes(pKey)
            Dim DataBytes() As Byte = UE.GetBytes(pValue)

            Dim HashDataBytes() As Byte
            Dim hmac As New HMACSHA1(KeyBytes)
            Dim cs As New CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write)
            cs.Write(DataBytes, 0, DataBytes.Length)
            cs.Close()

            Return Convert.ToBase64String(hmac.Hash)
        End Function

        Public Shared Function GetPasswordMaxLength(ByVal pConnectionString As String) As Long
            Call GetPasswordMaxLength(New EAD.DBCom(pConnectionString))
        End Function

        Public Shared Function GetPasswordMaxLength(ByVal pDBAccess As EAD.DBCom) As Long
            Return Val(pDBAccess.GetVal("SELECT LP_VALUE FROM LOGIN_POLICY WHERE LP_PARAM = 'PASSWORD_LENGTH_MAX'"))
        End Function

        Public Shared Function DuplicateString(ByVal intCnt As Integer, ByVal strRaw As String) As String
            Dim i As Integer
            Dim str As String = ""
            If intCnt > 0 Then
                For i = 0 To strRaw.Length - 1
                    str &= Strings.StrDup(intCnt, strRaw.Chars(i))
                Next
            End If

            Return str
        End Function

#Region " Select Distinct for DataTable "
        ' Created By Ai Chu
        Public Shared Function SelectDistinct(ByVal SourceTable As DataTable, ByVal ParamArray FieldNames() As String) As DataTable
            Dim lastValues() As Object
            Dim newTable As DataTable

            If FieldNames Is Nothing OrElse FieldNames.Length = 0 Then
                Throw New ArgumentNullException("FieldNames")
            End If

            lastValues = New Object(FieldNames.Length - 1) {}
            newTable = New DataTable

            For Each field As String In FieldNames
                newTable.Columns.Add(field, SourceTable.Columns(field).DataType)
            Next

            For Each Row As DataRow In SourceTable.Select("", String.Join(", ", FieldNames))
                If Not fieldValuesAreEqual(lastValues, Row, FieldNames) Then
                    newTable.Rows.Add(createRowClone(Row, newTable.NewRow(), FieldNames))

                    setLastValues(lastValues, Row, FieldNames)
                End If
            Next

            Return newTable
        End Function

        Public Shared Function fieldValuesAreEqual(ByVal lastValues() As Object, ByVal currentRow As DataRow, ByVal fieldNames() As String) As Boolean
            Dim areEqual As Boolean = True

            For i As Integer = 0 To fieldNames.Length - 1
                If lastValues(i) Is Nothing OrElse Not lastValues(i).Equals(currentRow(fieldNames(i))) Then
                    areEqual = False
                    Exit For
                End If
            Next

            Return areEqual
        End Function

        Public Shared Function createRowClone(ByVal sourceRow As DataRow, ByVal newRow As DataRow, ByVal fieldNames() As String) As DataRow
            For Each field As String In fieldNames
                newRow(field) = sourceRow(field)
            Next

            Return newRow
        End Function

        Public Shared Sub setLastValues(ByVal lastValues() As Object, ByVal sourceRow As DataRow, ByVal fieldNames() As String)
            For i As Integer = 0 To fieldNames.Length - 1
                lastValues(i) = sourceRow(fieldNames(i))
            Next
        End Sub
#End Region

    End Class

    Public Class AppGlobals

        'Name       : FillCodeTable
        'Author     : Moo
        'Descption  : Fill DropDownList for CODE_MSTR Table
        '             ie.COUNTRY,CURRENCY,UOM,GST,PAYMENT_TERM,SHIPMENT_CODE,SHIPMENT_TERM,PAYMENT_METHOD,STATE,BCM_MODE
        'Remark     : By Default filled only with ACTIVE code
        'ReturnValue: Return a DropDownList filled with data
        'LastUpadte : 01 Sep 2004
        'Version    : 1.00
        Public Sub FillCodeTable(ByRef pDropDownList As UI.WebControls.DropDownList, ByVal pCodeTableEnum As CodeTable, Optional ByVal pAllStatus As Boolean = False, Optional ByVal strRFQ As String = "", Optional ByVal pSelectOption As Boolean = True)
            Dim myData As New AppDataProvider
            Dim drvCodeMstr As DataView
            Dim strDefaultValue, strCountry, strCoyID, strSql As String
            Dim objDB As New EAD.DBCom

            If pAllStatus Then
                drvCodeMstr = myData.GetAllMasterCode(pCodeTableEnum, False)
                'Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_CODE", myData.GetAllMasterCode(pCodeTableEnum))
            Else
                drvCodeMstr = myData.GetMasterCodeByStatus(pCodeTableEnum, "N", False)
                'Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_CODE", myData.GetMasterCodeByStatus(pCodeTableEnum, "N"))
            End If

            Dim lstItem As New ListItem
            If Not drvCodeMstr Is Nothing Then
                Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_ABBR", drvCodeMstr)

                ' Add ---Select---
                If pSelectOption = True Then
                    lstItem.Value = ""
                    lstItem.Text = "---Select---"
                    strDefaultValue = lstItem.Text
                    pDropDownList.Items.Insert(0, lstItem)
                End If

                'pDropDownList.DataBind()
                '//System Default
                strCoyID = HttpContext.Current.Session("CompanyId")

                strCountry = objDB.GetVal("SELECT CM_COUNTRY FROM company_mstr WHERE CM_COY_ID = '" & strCoyID & "'")

                If Not IsDBNull(strCountry) Then
                    strDefaultValue = strCountry
                    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                Else
                    strDefaultValue = ""
                    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                End If

                'If Not IsDBNull(drvCodeMstr.Item(0)("CC_DEFAULT_CODE")) Then
                '    strDefaultValue = drvCodeMstr.Item(0)("CC_DEFAULT_CODE")
                '    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                'Else
                '    strDefaultValue = ""
                '    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                'End If

                '//Company Default


                Dim strVal As String
                Select Case pCodeTableEnum
                    Case CodeTable.PaymentTerm
                        strSql = "Select ISNULL(CM_PAYMENT_TERM,1) from COMPANY_MSTR WHERE CM_COY_ID='" & strCoyID & "'"
                        strVal = objDB.GetVal(strSql)
                        Common.SelDdl(strVal, pDropDownList, True, True)
                    Case CodeTable.PaymentMethod
                        strSql = "Select ISNULL(CM_PAYMENT_METHOD,1) from COMPANY_MSTR WHERE CM_COY_ID='" & strCoyID & "'"
                        strVal = objDB.GetVal(strSql)

                        'Jules 2018.11.07 to cater for vendors without default payment method.
                        If strVal = "1" Then
                            pDropDownList.SelectedIndex = 1
                        Else
                            Common.SelDdl(strVal, pDropDownList, True, True)
                        End If
                        'End modification.

                    Case CodeTable.Currency
                        If strRFQ = "RFQ" Then
                            strSql = "SELECT ISNULL(CM_CURRENCY_CODE,1) FROM COMPANY_MSTR WHERE CM_COY_ID='" & strCoyID & "'"
                            strVal = objDB.GetVal(strSql)
                            Common.SelDdl(strVal, pDropDownList, True, True)
                        End If
                End Select
                '//User Default
            Else
                '//no suppose to happen
            End If
        End Sub

        'Public Sub FillState(ByRef pDropDownList As UI.WebControls.DropDownList, _
        '                            ByVal pCodeTableEnum As Int32, ByVal pCountryCode As String, _
        '                            Optional ByVal pAllStatus As Boolean = False )
        Public Sub FillState(ByRef pDropDownList As UI.WebControls.DropDownList, ByVal pCountryCode As String)
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT CM.CODE_ABBR, CM.CODE_DESC, CC.CC_DEFAULT_CODE "
            strSql &= "FROM CODE_MSTR CM, CODE_CATEGORY CC "
            strSql &= "WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CM.CODE_VALUE='" & pCountryCode & "' AND CODE_DELETED='N' ORDER BY CODE_DESC"
            drw = objDB.GetView(strSql)
            'If pAllStatus Then
            '    drvCodeMstr = myData.GetAllMasterCode(pCodeTableEnum)
            '    'Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_CODE", myData.GetAllMasterCode(pCodeTableEnum))
            'Else
            '    drvCodeMstr = myData.GetMasterCodeByStatus(pCodeTableEnum, "N")
            '    'Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_CODE", myData.GetMasterCodeByStatus(pCodeTableEnum, "N"))
            'End If

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_ABBR", drw)

                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)

                'pDropDownList.DataBind()
                '//System Default
                If Not IsDBNull(drw.Item(0)("CC_DEFAULT_CODE")) Then
                    strDefaultValue = drw.Item(0)("CC_DEFAULT_CODE")
                    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                Else
                    strDefaultValue = ""
                    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                End If
                '//Company Default
                '//User Default
            Else
                ' Add ---Select---
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                'strDefaultValue = "---Not Applicable---"
                'pDropDownList.Items.Insert(0, lstItem)
                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        Public Sub FillIPPCompanyCategory(ByRef pDropDownList As UI.WebControls.DropDownList)
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT CM.CODE_ABBR, CM.CODE_DESC, CC.CC_DEFAULT_CODE " &
                    "FROM CODE_MSTR CM,CODE_CATEGORY CC WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CC.CC_DESC='IPPCompanyCategory' AND CODE_DELETED='N' AND CODE_DESC != '' ORDER BY CODE_DESC"
            drw = objDB.GetView(strSql)
            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_ABBR", drw)

                '//System Default
                If Not IsDBNull(drw.Item(0)("CC_DEFAULT_CODE")) Then
                    strDefaultValue = drw.Item(0)("CC_DEFAULT_CODE")
                    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                Else
                    strDefaultValue = ""
                    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                End If
            Else
                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                'strDefaultValue = "---Not Applicable---"
                'pDropDownList.Items.Insert(0, lstItem)
                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

            End If
            objDB = Nothing
        End Sub


        Public Sub FillLot(ByRef pDropDownList As UI.WebControls.DropDownList, ByVal pItemCode As String, ByVal pGRNCode As String, ByVal pPOCode As String, ByVal strLine As String)
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = " SELECT CAST(CONCAT(DOL_LOT_NO,'(',LM_LOCATION,':',LM_SUB_LOCATION,')') AS CHAR(1000)) AS DOL_LOT_NO, CAST(CONCAT(DOL_LOT_INDEX, '_', GL_LOCATION_INDEX) AS CHAR(1000)) AS DOL_LOT_INDEX FROM GRN_MSTR "
            strSql &= " INNER JOIN GRN_DETAILS ON GM_GRN_NO = GD_GRN_NO AND GD_B_COY_ID = GM_B_COY_ID "
            strSql &= " INNER JOIN GRN_LOT ON GL_GRN_NO = GD_GRN_NO AND GL_B_COY_ID = GD_B_COY_ID AND GL_PO_LINE = GD_PO_LINE "
            strSql &= " INNER JOIN PO_MSTR ON GM_PO_INDEX = POM_PO_INDEX AND GM_B_COY_ID = POM_B_COY_ID "
            strSql &= " INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID "
            strSql &= " INNER JOIN DO_MSTR ON DOM_DO_INDEX = GM_DO_INDEX "
            strSql &= " INNER JOIN DO_DETAILS ON DOD_S_COY_ID = DOM_S_COY_ID AND DOD_DO_NO = DOM_DO_NO AND DOD_PO_LINE = GL_PO_LINE "
            strSql &= " INNER JOIN DO_LOT ON DOL_COY_ID = DOM_S_COY_ID AND DOL_DO_NO = DOD_DO_NO AND DOL_LOT_INDEX = GL_LOT_INDEX "
            strSql &= " INNER JOIN LOCATION_MSTR ON LM_COY_ID = GD_B_COY_ID AND LM_LOCATION_INDEX = GL_LOCATION_INDEX "
            strSql &= " WHERE GD_GRN_NO = '" & pGRNCode & "' AND POM_PO_NO = '" & pPOCode & "' "
            strSql &= " AND POD_VENDOR_ITEM_CODE = '" & pItemCode & "' "
            strSql &= " AND GD_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND GD_PO_LINE = '" & strLine & "' "

            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "DOL_LOT_NO", "DOL_LOT_INDEX", drw)

                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)

                'pDropDownList.DataBind()
                '//System Default
                If Not IsDBNull(drw.Item(0)("DOL_LOT_NO")) Then
                    strDefaultValue = drw.Item(0)("DOL_LOT_INDEX")
                    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                Else
                    strDefaultValue = ""
                    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                End If
                '//Company Default
                '//User Default
            Else
                ' Add ---Select---
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                'strDefaultValue = "---Not Applicable---"
                'pDropDownList.Items.Insert(0, lstItem)
                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        Public Sub FillAssetGroup(ByRef pDropDownList As UI.WebControls.DropDownList)
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT AG_GROUP AS ASSET_GROUP, CONCAT(CONCAT(IFNULL(AG_GROUP, ''),' : '), IFNULL(AG_GROUP_DESC, '')) AS AG_GROUP_DESC " _
                    & " FROM ASSET_GROUP WHERE AG_GROUP_TYPE = 'A' AND AG_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " _
                    & " ORDER BY AG_GROUP "
            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "AG_GROUP_DESC", "ASSET_GROUP", drw)

                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)

            Else
                ' Add ---Select---
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                'strDefaultValue = "---Not Applicable---"
                'pDropDownList.Items.Insert(0, lstItem)
                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub
        Public Function FillAssetGroup() As DataSet
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String

            Dim objDB As New EAD.DBCom
            strSql = "SELECT AG_GROUP AS ASSET_GROUP, CONCAT(CONCAT(IFNULL(AG_GROUP, ''),' : '), IFNULL(AG_GROUP_DESC, '')) AS AG_GROUP_DESC " _
                    & " FROM ASSET_GROUP WHERE AG_GROUP_TYPE = 'A' AND AG_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " _
                    & " ORDER BY AG_GROUP "
            'drw = objDB.GetView(strSql)

            'pDropDownList.Items.Clear()
            'Dim lstItem As New ListItem
            'If Not drw Is Nothing Then
            '    Common.FillDdl(pDropDownList, "AG_GROUP_DESC", "ASSET_GROUP", drw)

            '    ' Add ---Select---
            '    lstItem.Value = ""
            '    lstItem.Text = "---Select---"
            '    strDefaultValue = lstItem.Text
            '    pDropDownList.Items.Insert(0, lstItem)

            'Else
            '    ' Add ---Select---
            '    lstItem.Value = "n.a."
            '    lstItem.Text = "---Not Applicable---"
            '    strDefaultValue = lstItem.Value
            '    pDropDownList.Items.Clear()
            '    pDropDownList.Items.Insert(0, lstItem)

            '    'strDefaultValue = "---Not Applicable---"
            '    'pDropDownList.Items.Insert(0, lstItem)
            '    Common.SelDdl(strDefaultValue, pDropDownList, True, True)

            '    '//no suppose to happen
            'End If

            Return objDB.FillDs(strSql)
        End Function

        Public Sub FillCommodityType(ByRef pDropDownList As UI.WebControls.DropDownList)
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT CT_ID,CT_NAME " _
                    & "FROM commodity_type WHERE CT_LAST_LVL=1 ORDER BY CT_NAME "
            drw = objDB.GetView(strSql)
            'If pAllStatus Then
            '    drvCodeMstr = myData.GetAllMasterCode(pCodeTableEnum)
            '    'Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_CODE", myData.GetAllMasterCode(pCodeTableEnum))
            'Else
            '    drvCodeMstr = myData.GetMasterCodeByStatus(pCodeTableEnum, "N")
            '    'Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_CODE", myData.GetMasterCodeByStatus(pCodeTableEnum, "N"))
            'End If

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CT_NAME", "CT_ID", drw)

                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)

                'pDropDownList.DataBind()
                '//System Default
                'If Not IsDBNull(drw.Item(0)("CC_DEFAULT_CODE")) Then
                '    strDefaultValue = drw.Item(0)("CC_DEFAULT_CODE")
                '    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                'Else
                'strDefaultValue = ""
                'Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                'End If
                '//Company Default
                '//User Default
            Else
                ' Add ---Select---
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                'strDefaultValue = "---Not Applicable---"
                'pDropDownList.Items.Insert(0, lstItem)
                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub
        Public Sub FillCommodityType1(ByRef pDropDownList As UI.WebControls.DropDownList)
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT CT_ID,CT_NAME " _
                    & "FROM commodity_type WHERE CT_LAST_LVL=0 ORDER BY CT_NAME "
            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CT_NAME", "CT_ID", drw)

                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)

            Else
                ' Add ---Select---
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub
        Public Sub FillTax(ByRef pDropDownList As UI.WebControls.DropDownList)
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT TAX_AUTO_NO,TAX_CODE,TAX_PERC," _
                    & "CASE WHEN TAX_PERC='' THEN (" & objDB.Concat(" ", "", "TAX_CODE", "TAX_PERC") & ") " _
                    & "ELSE (" & objDB.Concat2(" ", "", "TAX_CODE", "TAX_PERC", "'%'") & ") " _
                    & "END AS tax " _
                    & "FROM tax WHERE (TAX_CODE = 'GST' OR TAX_CODE = 'N/A') ORDER BY TAX_AUTO_NO"

            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "TAX_PERC", "TAX_AUTO_NO", drw)

                '' Add ---Select---
                'lstItem.Value = ""
                'lstItem.Text = "---Select---"
                'strDefaultValue = lstItem.Text
                'pDropDownList.Items.Insert(0, lstItem)

            Else
                ' Add ---Select---
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        Public Sub FillGST(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal blnSelect As Boolean = True, Optional ByVal strCompId As String = "")
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql, strCountry As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            If strCompId = "" Then
                strCompId = HttpContext.Current.Session("CompanyId")
            End If
            'strSql = "SELECT CM_COUNTRY FROM COMPANY_MSTR WHERE CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
            'strCountry = objDB.GetVal(strSql)

            strSql = "SELECT IF(TAX_PERC = '', CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST, CODE_ABBR " &
                    "FROM CODE_MSTR " &
                    "INNER JOIN TAX ON TAX_CODE = CODE_ABBR " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = '" & strCompId & "' " &
                    "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND TAX_COUNTRY_CODE = CM_COUNTRY "

            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "GST", "CODE_ABBR", drw)

                ' Add ---Select---
                If blnSelect = True Then
                    lstItem.Value = ""
                    lstItem.Text = "---Select---"
                    strDefaultValue = lstItem.Text
                    pDropDownList.Items.Insert(0, lstItem)
                End If
            Else
                ' Add ---Select---
                lstItem.Value = "N/A"
                lstItem.Text = "N/A"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        Public Sub FillTaxCode(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strRate As String = "", Optional ByVal strType As String = "", Optional ByVal strCategory As String = "eProcure", Optional ByVal blnSelect As Boolean = True, Optional ByVal blnDefaultVal As Boolean = False)
            Dim strDefaultValue As String
            Dim strSql, strCountry As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "SELECT * FROM TAX_MSTR " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "WHERE TM_DELETED <> 'Y' AND TM_CATEGORY = '" & strCategory & "' " &
                    "AND TM_COUNTRY_CODE = CM_COUNTRY "

            If strRate <> "" Then
                strSql &= " AND TM_TAX_RATE = '" & strRate & "' "
            End If

            If strType <> "" Then
                strSql &= " AND TM_TAX_TYPE = '" & strType & "' "
            End If

            If strCategory = "IPP" Then
                strSql &= " AND TM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            End If

            strSql &= " ORDER BY TM_TAX_CODE "

            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "TM_TAX_CODE", "TM_TAX_CODE", drw)
                If blnSelect = True Then
                    lstItem.Value = ""
                    lstItem.Text = "---Select---"

                    'Jules 2018.10.07 - SST
                    If strCategory = "eProcure" Then
                        If blnDefaultVal AndAlso strRate <> "" AndAlso strType <> "" Then
                            strDefaultValue = objDB.GetVal("SELECT TM_TAX_CODE FROM TAX_MSTR WHERE TM_TAX_RATE='" & strRate & "' AND TM_TAX_TYPE='" & strType & "' AND TM_CATEGORY = '" & strCategory & "' AND TM_DELETED='N' ")
                        Else
                            strDefaultValue = lstItem.Text
                        End If
                    Else
                        strDefaultValue = lstItem.Text
                    End If
                    'End modification.
                    pDropDownList.Items.Insert(0, lstItem)

                    'Jules 2018.10.08 - SST
                    If blnDefaultVal And strDefaultValue <> "" Then
                        Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                    End If
                    'End modification.
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

        Public Sub FillAddress(ByRef pDropDownList As UI.WebControls.DropDownList, ByVal strType As String, Optional ByVal pRole As FixedRole = FixedRole.All_Role)
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "SELECT '*' FROM USERS_ADDR WHERE UA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSql &= "AND UA_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
            strSql &= "AND UA_ADDR_TYPE = '" & strType & "' "
            strSql &= "AND UA_VIEW_OPTION = 1"
            If objDB.Exist(strSql) > 0 Then
                strSql = "SELECT AM_ADDR_CODE FROM ADDRESS_MSTR "
                strSql &= "WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strSql &= "AND AM_ADDR_TYPE = '" & strType & "' "
                strSql &= "ORDER BY AM_ADDR_CODE "
            Else
                'Jules 2018.07.16 - Should tie to address_mstr to ensure the correct address type.
                strSql = "SELECT UA_ADDR_CODE AS AM_ADDR_CODE FROM USERS_ADDR "
                strSql &= "INNER JOIN ADDRESS_MSTR ON AM_ADDR_CODE = UA_ADDR_CODE AND AM_COY_ID = UA_COY_ID AND UA_ADDR_TYPE = AM_ADDR_TYPE "
                strSql &= "WHERE UA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strSql &= "AND UA_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
                strSql &= "AND UA_ADDR_TYPE = '" & strType & "' "
                strSql &= "AND UA_VIEW_OPTION = 0 "
                strSql &= "ORDER BY AM_ADDR_CODE "
            End If
            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "AM_ADDR_CODE", "AM_ADDR_CODE", drw)
            Else
                '//no suppose to happen
            End If
            ' Add ---Select---
            lstItem.Value = ""
            lstItem.Text = "---Select---"
            strDefaultValue = lstItem.Text
            pDropDownList.Items.Insert(0, lstItem)
            objDB = Nothing
        End Sub

        Public Sub FillOneVendor(ByRef pDropDownList As UI.WebControls.DropDownList, ByVal sVendorId As String)
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView, sProductCode As String = ""
            Dim objDB As New EAD.DBCom

            strSql = "SELECT CM_COY_ID, CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = '" & sVendorId & "' "
            drw = objDB.GetView(strSql)
            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CM_COY_NAME", "CM_COY_ID", drw)
            Else
                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        Public Function GetVenPrefViaProductCode(ByVal sProd As String) As DataSet
            Dim strSql As String
            Dim sProductCode As String = sProd
            Dim objDB As New EAD.DBCom

            strSql = "SELECT pm_PREFER_S_COY_ID, pm_1ST_S_COY_ID, pm_2ND_S_COY_ID, pm_3RD_S_COY_ID FROM product_mstr "
            strSql &= "WHERE pm_product_code IN (" & sProductCode & ")"

            Return objDB.FillDs(strSql)
        End Function

        Public Function GetVenViaProductCode(ByVal sProd As String, ByVal sProdGrp As String) As DataSet
            Dim strSql As String
            Dim sProductCode As String = sProd
            Dim objDB As New EAD.DBCom

            strSql = " SELECT * "
            strSql &= " FROM CONTRACT_DIST_COY "
            strSql &= " INNER JOIN CONTRACT_DIST_MSTR ON CDC_GROUP_INDEX = CDM_GROUP_INDEX "
            strSql &= " INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX=CDM_GROUP_INDEX"
            strSql &= " LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE"
            strSql &= " WHERE CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'"
            strSql &= " AND PM_PRODUCT_CODE IN (" & sProductCode & ")"
            strSql &= " AND CDM_GROUP_INDEX = '" & sProdGrp & "'"

            Return objDB.FillDs(strSql)
        End Function

        Public Sub FillVendorViaProductCode(ByRef pDropDownList As UI.WebControls.DropDownList, ByVal sPONO As String, Optional ByVal sPROD As String = "", Optional ByVal sPOMode As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView, sProductCode As String = ""
            Dim objDB As New EAD.DBCom


            strSql = "SELECT POD_PRODUCT_CODE FROM po_details WHERE POD_PO_NO = '" & sPONO & "' "
            Dim tDS As DataSet = objDB.FillDs(strSql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                sProductCode = sProductCode & "'" & tDS.Tables(0).Rows(j).Item("POD_PRODUCT_CODE") & "', "
            Next j

            If sProductCode = "" Then
                sProductCode = sPROD
            Else
                sProductCode = Mid(sProductCode, 1, Len(sProductCode) - 2)
            End If

            If sPOMode <> "cc" Then
                If sProductCode <> "" Then
                    strSql = "SELECT pm_PREFER_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = pm_PREFER_S_COY_ID) AS BB FROM product_mstr "
                    strSql &= "WHERE pm_product_code IN (" & sProductCode & ") AND PM_PREFER_S_COY_ID <> '' "
                    strSql &= "UNION SELECT pm_1ST_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_1ST_S_COY_ID) AS BB FROM product_mstr "
                    strSql &= "WHERE pm_product_code IN (" & sProductCode & ") AND PM_1ST_S_COY_ID <> '' "
                    strSql &= "UNION SELECT pm_2ND_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_2ND_S_COY_ID) AS BB FROM product_mstr "
                    strSql &= "WHERE pm_product_code IN (" & sProductCode & ") AND PM_2ND_S_COY_ID <> '' "
                    strSql &= "UNION SELECT pm_3RD_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_3RD_S_COY_ID) AS BB FROM product_mstr "
                    strSql &= "WHERE pm_product_code IN (" & sProductCode & ") AND PM_3RD_S_COY_ID <> '' "
                    strSql &= "GROUP BY AA, BB"
                Else
                    strSql = "SELECT pm_PREFER_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = pm_PREFER_S_COY_ID) AS BB FROM product_mstr "
                    strSql &= "WHERE PM_PREFER_S_COY_ID <> '' "
                    strSql &= "UNION SELECT pm_1ST_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_1ST_S_COY_ID) AS BB FROM product_mstr "
                    strSql &= "WHERE PM_1ST_S_COY_ID <> '' "
                    strSql &= "UNION SELECT pm_2ND_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_2ND_S_COY_ID) AS BB FROM product_mstr "
                    strSql &= "WHERE PM_2ND_S_COY_ID <> '' "
                    strSql &= "UNION SELECT pm_3RD_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_3RD_S_COY_ID) AS BB FROM product_mstr "
                    strSql &= "WHERE PM_3RD_S_COY_ID <> '' "
                    strSql &= "GROUP BY AA, BB"
                End If
            Else
                If sProductCode <> "" Then
                    strSql = "SELECT CDM_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = CDM_S_COY_ID) AS BB FROM CONTRACT_DIST_MSTR, CONTRACT_DIST_ITEMS "
                    strSql &= "WHERE CDM_GROUP_INDEX = CDI_GROUP_INDEX AND CDI_PRODUCT_CODE IN (" & sProductCode & ") AND CDM_S_COY_ID <> '' "
                    strSql &= "GROUP BY AA, BB"
                Else
                    strSql = "SELECT CDM_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = CDM_S_COY_ID) AS BB FROM CONTRACT_DIST_MSTR, CONTRACT_DIST_ITEMS "
                    strSql &= "WHERE CDM_GROUP_INDEX = CDI_GROUP_INDEX AND CDM_S_COY_ID <> '' "
                    strSql &= "GROUP BY AA, BB"
                End If


                'strSql = "SELECT PM_LAST_TXN_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PM_LAST_TXN_S_COY_ID) AS BB FROM PRODUCT_MSTR "
                'strSql &= "WHERE PM_VENDOR_ITEM_CODE IN (" & sProductCode & ") AND PM_LAST_TXN_S_COY_ID <> '' "
                'strSql &= "GROUP BY AA, BB"
            End If
            If sPOMode = "bc" Then
                strSql = "SELECT "
                strSql &= "(SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_COY_ID = POM_S_COY_ID) AS AA, "
                strSql &= "(SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = POM_S_COY_ID) AS BB "
                strSql &= "FROM(PO_MSTR, PO_DETAILS) "
                strSql &= "WHERE POM_PO_NO = POD_PO_NO AND POD_PRODUCT_CODE IN (" & sProductCode & ") AND POD_PO_NO = '" & sPONO & "' "
                strSql &= "GROUP BY AA, BB "

            End If

            drw = objDB.GetView(strSql)

            'If CInt(drw.Count.ToString) < 0 Then
            'strSql = "SELECT POM_S_COY_ID as AA, POM_S_COY_NAME as BB FROM PO_MSTR WHERE POM_PO_NO = '" & sPONO & "' "
            'drw = objDB.GetView(strSql)
            'End If

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "BB", "AA", drw)
            Else
                '//no suppose to happen
            End If
            ' Add ---Select---
            lstItem.Value = ""
            lstItem.Text = "---Select---"
            strDefaultValue = lstItem.Text
            pDropDownList.Items.Insert(0, lstItem)
            objDB = Nothing
        End Sub

        'Name       : FillScreen
        'Author     : kk
        'Descption  : Fill DropDownList for Menu
        'Remark     : 
        'ReturnValue: Return a DropDownList filled with data
        'LastUpadte : 04 Nov 2004
        'Version    : 1.00
        Public Sub FillScreen(ByRef pDropDownList As UI.WebControls.DropDownList)
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT DISTINCT(MM_GROUP),MM_MENU_PARENT FROM MENU_MSTR "
            strSql &= "ORDER BY MM_MENU_PARENT "
            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "MM_GROUP", "MM_MENU_PARENT", drw)

                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        'Name       : FillFixedRole
        'Author     : kk
        'Descption  : Fill DropDownList for Menu
        'Remark     : Put this at Global, advantage for distribute to 2 copy
        'ReturnValue: Return a DropDownList filled with data
        'LastUpadte : 04 Nov 2004
        'Version    : 1.00
        Public Sub FillFixedRole(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As EAD.DBCom
            If strConn = "" Then
                objDB = New EAD.DBCom
            Else
                objDB = New EAD.DBCom(strConn)
            End If

            'Michelle (15/9/2011) - Does not display the Super Admin role (837)
            'Joon (16/9/2011) - Does not display the Consolidator role (837)
            strSql = "SELECT DISTINCT(FR_ROLE_ID) FROM FIXED_ROLE WHERE fr_role_id NOT IN ('Super Admin','Consolidator')"
            strSql &= " ORDER BY FR_ROLE_ID "
            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "FR_ROLE_ID", "FR_ROLE_ID", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
                lstItem.Text = "---Select---"
                pDropDownList.Items.Insert(0, lstItem)
            End If
            objDB = Nothing
        End Sub

        Public Sub FillCompany(ByRef pDropDownList As UI.WebControls.DropDownList, ByVal Type As String)
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT CM_COY_ID, CM_COY_NAME "
            strSql &= "FROM COMPANY_MSTR "
            If Type = "HubAdmin" Then
                strSql &= "WHERE CM_COY_ID <> '" & HttpContext.Current.Session("CompanyIdToken") & "' "
            Else
                strSql &= "WHERE CM_COY_ID <> '" & HttpContext.Current.Session("CompanyID") & "' "
            End If
            strSql &= "AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "
            strSql &= "ORDER BY CM_COY_NAME"

            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CM_COY_NAME", "CM_COY_ID", drw)

                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)

                'pDropDownList.DataBind()
                '//System Default
                'If Not IsDBNull(drw.Item(0)("CC_DEFAULT_CODE")) Then
                '    strDefaultValue = drw.Item(0)("CC_DEFAULT_CODE")
                '    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                'Else
                '    strDefaultValue = ""
                '    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                'End If
                '//Company Default
                '//User Default
            Else
                strDefaultValue = "---Not Applicable---"
                pDropDownList.Items.Insert(0, strDefaultValue)
                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
        End Sub


        Public Function GetCodeTableView(ByVal pCodeTableEnum As Int32, Optional ByVal pAllStatus As Boolean = False) As DataView
            Dim myData As New AppDataProvider
            Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            If pAllStatus Then
                drvCodeMstr = myData.GetAllMasterCode(pCodeTableEnum, False)
                'Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_CODE", myData.GetAllMasterCode(pCodeTableEnum))
            Else
                drvCodeMstr = myData.GetMasterCodeByStatus(pCodeTableEnum, "N", False)
                'Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_CODE", myData.GetMasterCodeByStatus(pCodeTableEnum, "N"))
            End If
            Return drvCodeMstr
            '//Company Default
            '//User Default
        End Function

        Public Sub GetLatestDocNo(ByVal companyId As String, ByVal pDocType As String, ByRef pQuery() As String, ByRef strDocNo As String, ByRef strDocPrefix As String, Optional ByVal blnPreviewPO As Boolean = False, Optional ByVal intIncrementNo As Integer = 1, Optional ByRef strNewNo As String = "")
            Dim strSql, strPrefix, strLastUsedNo, strReplicate As String
            Dim intLeadingZero, intLen, intLoop As Integer
            Dim strCoyId As String = companyId
            Dim objDB As New EAD.DBCom
            Dim tDS As DataSet
            Dim blnFound, blnFound1 As Boolean
            blnFound = False
            blnFound1 = False
            'strCoyId = "demo"
            strLastUsedNo = "0"
            strPrefix = ""
            strSql = "SELECT * FROM COMPANY_PARAM WHERE CP_COY_ID='" & strCoyId &
            "' AND CP_PARAM_TYPE='" & pDocType & "'"
            tDS = objDB.FillDs(strSql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                'blnFound = True
                If UCase(Common.parseNull(tDS.Tables(0).Rows(j).Item("CP_PARAM_NAME"))) = "PREFIX" Then
                    strPrefix = Common.parseNull(tDS.Tables(0).Rows(j).Item("CP_PARAM_VALUE"))
                    blnFound = True
                ElseIf UCase(Common.parseNull(tDS.Tables(0).Rows(j).Item("CP_PARAM_NAME"))) = "LAST USED NO" Then
                    'strLastUsedNo = Common.parseNull(drData("CP_PARAM_VALUE"), "0")
                    If Not IsDBNull(tDS.Tables(0).Rows(j).Item("CP_PARAM_VALUE")) Then
                        If tDS.Tables(0).Rows(j).Item("CP_PARAM_VALUE") = "" Then
                            strLastUsedNo = "0"
                        Else
                            strLastUsedNo = tDS.Tables(0).Rows(j).Item("CP_PARAM_VALUE")
                        End If
                    Else
                        strLastUsedNo = "0"
                    End If
                    blnFound1 = True
                End If
            Next
            '//Len of the string before convert to integer
            '//00001 = return 5
            Dim strAry(0) As String
            If Not blnFound Then
                strSql = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_NAME,CP_PARAM_VALUE,CP_PARAM_TYPE) VALUES('" &
                strCoyId & "','Prefix','" & strPrefix & "','" & pDocType & "')"
                Common.Insert2Ary(strAry, strSql)
            End If

            If Not blnFound1 Then
                strSql = "INSERT INTO COMPANY_PARAM(CP_COY_ID,CP_PARAM_NAME,CP_PARAM_VALUE,CP_PARAM_TYPE) VALUES('" &
                strCoyId & "','Last Used No','" & strLastUsedNo & "','" & pDocType & "')"
                Common.Insert2Ary(strAry, strSql)
            End If

            If Not blnFound Or Not blnFound1 Then
                objDB.BatchExecute(strAry)
                objDB = Nothing
            End If
            If strLastUsedNo.Substring(0, 1) = "0" Then
                intLen = strLastUsedNo.Length
                strLastUsedNo = Convert.ToInt32(strLastUsedNo) + intIncrementNo
                '//after perform adding, leading zero lost
                intLeadingZero = intLen - strLastUsedNo.Length
                '//add back the leading zero
                strReplicate = Strings.StrDup(intLeadingZero, "0")
            Else
                strLastUsedNo = Convert.ToInt64(strLastUsedNo) + intIncrementNo
                intLeadingZero = 0
                strReplicate = Strings.StrDup(intLeadingZero, "0")
            End If

            'For intLoop = 0 To intLeadingZero - 1
            '    strReplicate = strReplicate & "0"
            'Next
            '//update database
            strDocPrefix = strPrefix
            strDocNo = strPrefix & strReplicate & strLastUsedNo
            strNewNo = strReplicate & strLastUsedNo

            If Not blnPreviewPO Then
                strSql = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE='" & strReplicate & strLastUsedNo &
                "' WHERE CP_COY_ID='" & strCoyId & "' AND CP_PARAM_TYPE='" & pDocType &
                "' AND CP_PARAM_NAME='LAST USED NO'"
                'objDB.Execute(strSql)
                Common.Insert2Ary(pQuery, strSql)
            End If
            '//build doc no
            'GetLatestDocNo = strPrefix & strReplicate & strLastUsedNo
            objDB = Nothing
        End Sub

        Public Function getCodeDesc(ByVal pCodeTableEnum As CodeTable, ByVal strCodeVal As String)
            Dim strCodeCat As String = System.Enum.GetName(GetType(CodeTable), pCodeTableEnum)
            Dim strSql, strResult As String
            Dim objDB As New EAD.DBCom
            'lsSql = "SELECT CM.CODE_ABBR,CM.CODE_DESC,CC.CC_DEFAULT_CODE FROM CODE_MSTR CM,CODE_CATEGORY CC WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CC.CC_DESC='" & strCodeType & "' AND CODE_DELETED='" & pStatus & "' ORDER BY CONVERT(INT,CODE_VALUE)"
            strSql = "SELECT CODE_DESC FROM CODE_MSTR CM, CODE_CATEGORY CC WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CC.CC_DESC='" &
            strCodeCat & "' AND CODE_ABBR='" & strCodeVal & "'"
            Dim tDS As DataSet = objDB.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                strResult = Common.parseNull(tDS.Tables(0).Rows(0).Item("CODE_DESC"), strCodeVal)
            Else
                strResult = strCodeVal
            End If

            objDB = Nothing
            Return strResult
        End Function

        'Function GetErrorMessage(ByVal strErrNo As String) As String
        '    Dim strSQL As String
        '    Dim ds As DataSet
        '    Dim objDB As New EAD.DBCom
        Function GetErrorMessage(ByVal strErrNo As String, Optional ByVal path As String = "eprocurePath") As String
            Dim strSQL, field As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom(ConfigurationManager.AppSettings(path))
            strSQL = "SELECT MM_MESSAGE " _
                    & "FROM MESSAGE_MSTR " _
                    & "WHERE MM_CODE ='" & strErrNo & "'"
            ds = objDB.FillDs(strSQL)
            If ds.Tables(0).Rows.Count > 0 Then
                GetErrorMessage = ds.Tables(0).Rows(0).Item("MM_MESSAGE")
            End If
            ds = Nothing
        End Function

        Public Sub FillIBSGLCode(ByRef pDropDownList As UI.WebControls.DropDownList)
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            Dim strCoyId As String
            strCoyId = HttpContext.Current.Session("CompanyId")
            'retrieve cbg_b_gl_code from company_b_gl_code table 
            'where cbg_status = ‘A?and cbg_b_coy_id = company id of the login user.
            'strSql = "SELECT TAX_AUTO_NO,TAX_CODE,TAX_PERC," _
            '        & "CASE WHEN TAX_PERC='' THEN (" & objDB.Concat(" ", "", "TAX_CODE", "TAX_PERC") & ") " _
            '        & "ELSE (" & objDB.Concat2(" ", "", "TAX_CODE", "TAX_PERC", "'%'") & ") " _
            '        & "END AS tax " _
            '        & "FROM tax ORDER BY TAX_AUTO_NO"
            'strSql = "SELECT CBG_B_GL_CODE, CBG_B_GL_DESC FROM company_b_gl_code " _
            '        & "WHERE cbg_status = 'A' AND cbg_b_coy_id = '" & strCoyId & "' ORDER BY CBG_B_GL_CODE ASC"

            strSql = "SELECT CONCAT(CBG_B_GL_CODE ,':', CBG_B_GL_DESC) AS CBG_B_GL_CODE_DESC, CBG_B_GL_CODE FROM company_b_gl_code " _
                    & "WHERE cbg_status = 'A' AND cbg_b_coy_id = '" & strCoyId & "' ORDER BY CBG_B_GL_CODE ASC "

            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CBG_B_GL_CODE_DESC", "CBG_B_GL_CODE", drw)

                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)

            Else
                ' Add ---Select---
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        Public Sub FillBankCode(ByRef pDropDownList As UI.WebControls.DropDownList, ByVal strPayMethod As String)
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String = ""
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            Dim strCoyId As String
            Dim lstItem As New ListItem

            'Zulham 25032015 Case 8603
            'strCoyId = HttpContext.Current.Session("CompanyId")
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyId")
            Else
                strCoyId = strDefIPPCompID
            End If

            Select Case strPayMethod
                Case "BC"
                    'Zulham 23112018
                    strSql = "SELECT '*' FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' AND bc_status = 'A' AND bc_usage = 'BC' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                Case "CASA"
                    strSql = "SELECT '*' FROM IPP_PARAMETER WHERE IP_COY_ID = '" & strCoyId & "' AND ip_param = 'CASA_BANKCODE' AND ip_param_value <> ''"
                Case "IBG"
                    strSql = "SELECT '*' FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' AND bc_status = 'A' AND bc_usage = 'IBG' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                Case "TT"
                    strSql = "SELECT '*' FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' AND bc_status = 'A' AND bc_usage = 'TT' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                    'Michelle (26/2/2013) - Issue 1857
                Case "RENTAS"
                    strSql = "SELECT '*' FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' AND bc_status = 'A' AND bc_usage = 'RT' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                    'Zulham 9/3/2015 IPP GST Stage 2B
                Case "NOSTRO"
                    strSql = "SELECT '*' FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' AND bc_status = 'A' AND bc_usage = 'NT' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                    'Zulham 17102018 - PAMB
                Case "CO"
                    strSql = "SELECT '*' FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' AND bc_status = 'A' AND bc_usage = 'CO' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                Case "BD"
                    strSql = "SELECT '*' FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' AND bc_status = 'A' AND bc_usage = 'BD' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"

            End Select

            If objDB.Exist(strSql) Then

                Select Case strPayMethod
                    Case "BC"
                        'Zulham 23112018
                        strSql = "SELECT bc_bank_code 'IP_PARAM_VALUE', bc_bank_name FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' " _
                                & "AND bc_status = 'A' AND bc_usage = 'BC' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                    Case "CASA"
                        strSql = "SELECT IP_PARAM_VALUE, IP_PARAM_VALUE FROM ipp_parameter WHERE ip_param = 'CASA_BANKCODE' and ip_coy_id = '" & strCoyId & "' "
                    Case "IBG"
                        strSql = "SELECT bc_bank_code, bc_bank_name FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' " _
                                & "AND bc_status = 'A' AND bc_usage = 'IBG' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                    Case "TT"
                        strSql = "SELECT bc_bank_code, bc_bank_name FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' " _
                                & "AND bc_status = 'A' AND bc_usage = 'TT' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                    Case "RENTAS"
                        strSql = "SELECT bc_bank_code, bc_bank_name FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' " _
                                & "AND bc_status = 'A' AND bc_usage = 'RT' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                        'Zulham 9/3/2015 IPP GST Stage 2B
                    Case "NOSTRO"
                        strSql = "SELECT bc_bank_code, bc_bank_name FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' " _
                                & "AND bc_status = 'A' AND bc_usage = 'NT' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                        'Zulham 17102018 - PAMB
                    Case "CO"
                        strSql = "SELECT bc_bank_code, bc_bank_name FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' " _
                                & "AND bc_status = 'A' AND bc_usage = 'CO' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                    Case "BD"
                        strSql = "SELECT bc_bank_code, bc_bank_name FROM bank_code WHERE bc_coy_id = '" & strCoyId & "' " _
                                & "AND bc_status = 'A' AND bc_usage = 'BD' and bc_bank_code <> '" & HttpContext.Current.Session("CompanyID") & "'"
                End Select

                drw = objDB.GetView(strSql)

                pDropDownList.Items.Clear()
                'Dim lstItem As New ListItem
                'Zulham 9/3/2015 IPP GST Stage 2B
                'Zulham 17102018 - PAMB 
                If strPayMethod = "IBG" Or strPayMethod = "TT" Or strPayMethod = "RENTAS" Or strPayMethod.ToUpper = "NOSTRO" _
                Or strPayMethod = "CO" Or strPayMethod = "BD" Then
                    Common.FillDdl(pDropDownList, "BC_BANK_CODE", "BC_BANK_CODE", drw)

                    ' Add ---Select---
                    lstItem.Value = ""
                    lstItem.Text = "---Select---"
                    strDefaultValue = lstItem.Text
                    pDropDownList.Items.Insert(0, lstItem)
                ElseIf strPayMethod = "BC" Or strPayMethod = "CASA" Then
                    Common.FillDdl(pDropDownList, "IP_PARAM_VALUE", "IP_PARAM_VALUE", drw)

                    ' Add ---Select---
                    lstItem.Value = ""
                    lstItem.Text = "---Select---"
                    strDefaultValue = lstItem.Text
                    pDropDownList.Items.Insert(0, lstItem)
                Else
                    ' Add ---Not Applicable---
                    lstItem.Value = "n.a."
                    lstItem.Text = "---Not Applicable---"
                    strDefaultValue = lstItem.Value
                    pDropDownList.Items.Clear()
                    pDropDownList.Items.Insert(0, lstItem)

                    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                End If
            Else
                ' Add ---Not Applicable---
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        Public Sub FillIPPPaymentMethod(ByRef pDropDownList As UI.WebControls.DropDownList)
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            Dim strCoyId As String
            strCoyId = HttpContext.Current.Session("CompanyId")
            'Zulham 17102018 - PAMB
            If Not strCoyId.ToUpper = "PAMB" Then
                strSql = "SELECT CODE_DESC, CODE_ABBR FROM CODE_MSTR CM WHERE CODE_CATEGORY='IPPPM' AND CODE_DELETED = 'N' "
            Else
                strSql = "SELECT CODE_DESC, CODE_ABBR FROM CODE_MSTR CM WHERE CODE_CATEGORY='IPPPM' AND CODE_ABBR IN ('TT','IBG','CO','BD','BC') AND CODE_DELETED = 'N' "
            End If
            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_ABBR", drw)

                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)

            Else
                ' Add ---Select---
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        Public Sub FillIPPBranchCode(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal blnFilterAllCoy As Boolean = False)
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            Dim strCoyId As String
            strCoyId = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT CONCAT(CBM_BRANCH_CODE, ':', CBM_BRANCH_NAME) AS CBM_BRANCH_CODE_DESC, CBM_BRANCH_CODE "

            If blnFilterAllCoy = True Then
                strSql &= "FROM COMPANY_BRANCH_MSTR WHERE CBM_COY_ID IN ('" & HttpContext.Current.Session("CompanyID") & "') AND CBM_STATUS = 'A' " &
                    "GROUP BY CBM_BRANCH_CODE " &
                    "ORDER BY CBM_BRANCH_CODE ASC "
            Else
                strSql &= "FROM COMPANY_BRANCH_MSTR WHERE CBM_COY_ID = '" & strCoyId & "' AND CBM_STATUS = 'A' " &
                    "ORDER BY CBM_BRANCH_CODE ASC "
            End If

            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CBM_BRANCH_CODE_DESC", "CBM_BRANCH_CODE", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)
                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        Public Sub FillIPPCostCentre(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal blnFilterAllCoy As Boolean = False)
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            Dim strCoyId As String
            strCoyId = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT CONCAT(CC_CC_CODE, ':', CC_CC_DESC) AS CC_CC_CODE_DESC, CC_CC_CODE "

            If blnFilterAllCoy = True Then
                strSql &= "FROM COST_CENTRE WHERE CC_COY_ID IN ('" & HttpContext.Current.Session("CompanyID") & "') AND CC_STATUS = 'A' " &
                        "GROUP BY CC_CC_CODE " &
                        "ORDER BY CC_CC_CODE ASC "
            Else
                strSql &= "FROM COST_CENTRE WHERE CC_COY_ID = '" & strCoyId & "' AND CC_STATUS = 'A' " &
                        "ORDER BY CC_CC_CODE ASC "
            End If

            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CC_CC_CODE_DESC", "CC_CC_CODE", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)

            Else
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        'Jules 2018.05.03 - PAMB Scrum 2 - P2P Fund Type, Person Code and Project Code
        Public Sub FillAnalysisCode(ByVal strDeptCode As String, ByRef pDropDownList As UI.WebControls.DropDownList)
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT AC_ANALYSIS_CODE, CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE, ''),' : '), IFNULL(AC_ANALYSIS_CODE_DESC, '')) AS AC_ANALYSIS_CODE_DESC " _
                        & "FROM analysis_code " _
                        & "WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' AND AC_DEPT_CODE = '" & strDeptCode & "' AND AC_STATUS = 'O' " _
                        & "ORDER BY AC_ANALYSIS_CODE "

            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "AC_ANALYSIS_CODE_DESC", "AC_ANALYSIS_CODE", drw)

                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)

            Else
                ' Add ---Select---
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                'strDefaultValue = "---Not Applicable---"
                'pDropDownList.Items.Insert(0, lstItem)
                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub
        'End modification

        Public Sub FillAnalysisCodeType(ByRef pDropDownList As UI.WebControls.DropDownList, ByRef coyId As String)
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            'Jules 2018.07.11 - Ignore L6 (Tax Code) and L7 (Cost Centre)
            strSql = "SELECT DISTINCT ac_dept_code, " &
                     "Case " &
                     "WHEN ac_dept_code = 'L1' THEN 'Fund Type' " &
                     "WHEN ac_dept_code = 'L2' THEN 'Product Type' " &
                     "WHEN ac_dept_code = 'L3' THEN 'Channel' " &
                     "WHEN ac_dept_code = 'L4' THEN 'Reinsurance Company' " &
                     "WHEN ac_dept_code = 'L5' THEN 'Asset Fund' " &
                     "WHEN ac_dept_code = 'L8' THEN 'Project Code' " &
                     "WHEN ac_dept_code = 'L9' THEN 'Person Code' " &
                     "End " &
                     "AS 'Description' " &
                     "From analysis_code where ac_b_coy_id ='" & coyId & "' AND ac_dept_code NOT IN ('L6','L7')" &
                     "Order By ac_dept_code "

            drw = objDB.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "Description", "ac_dept_code", drw)
            Else
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)
                Common.SelDdl(strDefaultValue, pDropDownList, True, True)
            End If
            objDB = Nothing
        End Sub

        Public Sub FillAnalysisCodeType(ByRef pCheckBoxList As UI.WebControls.CheckBoxList, ByRef coyId As String)
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT DISTINCT ac_dept_code, " &
                     "Case " &
                     "WHEN ac_dept_code = 'L1' THEN 'Fund Type' " &
                     "WHEN ac_dept_code = 'L2' THEN 'Product Type' " &
                     "WHEN ac_dept_code = 'L3' THEN 'Channel' " &
                     "WHEN ac_dept_code = 'L4' THEN 'Reinsurance Company' " &
                     "WHEN ac_dept_code = 'L5' THEN 'Asset Fund' " &
                     "WHEN ac_dept_code = 'L6' THEN 'Tax Code' " &
                     "WHEN ac_dept_code = 'L7' THEN 'Cost Centre' " &
                     "WHEN ac_dept_code = 'L8' THEN 'Project Code' " &
                     "WHEN ac_dept_code = 'L9' THEN 'Person Code' " &
                     "End " &
                     "AS 'Description' " &
                     "From analysis_code where ac_b_coy_id ='" & coyId & "'" &
                     "Order By ac_dept_code "

            drw = objDB.GetView(strSql)

            pCheckBoxList.Items.Clear()
            Dim lstItem As New ListItem
            pCheckBoxList.DataSource = drw
            pCheckBoxList.DataTextField = "Description"
            pCheckBoxList.DataValueField = "ac_dept_code"
            pCheckBoxList.DataBind()
            objDB = Nothing
        End Sub

        'Jules 2018.07.17
        Public Function GetAnalysisCodeByDept(ByVal strDeptCode As String, Optional blnSwitch As Boolean = False) As DataSet
            Dim strSql As String
            Dim objDB As New EAD.DBCom

            If blnSwitch Then 'Display "Description : Code"
                strSql = "SELECT AC_ANALYSIS_CODE, IF(AC_ANALYSIS_CODE='N/A',' N/A',CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, ''))) AS AC_ANALYSIS_CODE_DESC  " _
                        & "FROM analysis_code " _
                        & "WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' AND AC_DEPT_CODE = '" & strDeptCode & "' AND AC_STATUS = 'O' " _
                        & "ORDER BY AC_ANALYSIS_CODE_DESC "
            Else 'original codes
                strSql = "SELECT AC_ANALYSIS_CODE, CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE, ''),' : '), IFNULL(AC_ANALYSIS_CODE_DESC, '')) AS AC_ANALYSIS_CODE_DESC " _
                        & "FROM analysis_code " _
                        & "WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' AND AC_DEPT_CODE = '" & strDeptCode & "' AND AC_STATUS = 'O' " _
                        & "ORDER BY AC_ANALYSIS_CODE "
            End If

            Return objDB.FillDs(strSql)
        End Function

        Public Function getCurrencyAbbr() As DataSet
            Dim sql As String
            Dim objdb As New EAD.DBCom

            sql = "SELECT CODE_ABBR
                    FROM code_mstr
                    WHERE code_category = 'CU'
                    AND CODE_DELETED  = 'N'"

            Return objdb.FillDs(sql)

        End Function
    End Class

    Public Class Result
        Public strSql As String
        Public ds As DataSet
    End Class
End Namespace