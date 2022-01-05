(function ($) {
 "use strict";
 
	/*----------------------
		wizard
	 -----------------------*/
	$('#rootwizard').bootstrapWizard({
		'nextSelector': '.button-next', 'previousSelector': '.button-previous', 'firstSelector': '.button-first', 'lastSelector': '.button-last',
        onNext: function (tab, navigation, index) {
            if (index == 1) {
                
                return validateFirstStep();
            } else if (index == 2) {
                
                return validateSecondStep();
            } else if (index == 3) {

                return validateThirdStep();
            } //etc.

        },
        onTabClick: function (tab, navigation, index) {
            // Disable the posibility to click on tabs
            return false;
        },
    });

    function validateFirstStep() {
        $('.errMsg').hide();
        var name = $('#txtName').val();        
        var email = $('#txtEmail').val();
        var password = $('#txtPassword').val();
        var confirmPassword = $('#txtConfirmPassword').val();
        var mobileNo = $('#txtMobileNo').val();

        if (name == null || name == '') {
            $('#txtName').focus();
            $('#txtName').parent().parent('div').find('.errMsg').empty();
            $('#txtName').parent().parent('div').append("<div class='errMsg' style='color:#cc0000;margin-top:5px;'><i>Name is required.</i></div>");
            return false;
        } else if (email == null || email == '') {
            $('#txtEmail').focus();
            $('#txtEmail').parent().parent('div').find('.errMsg').empty();
            $('#txtEmail').parent().parent('div').append("<div class='errMsg' style='color:#cc0000;margin-top:5px;'><i>Email is required.</i></div>");
            return false;
        } else if (password == null || password == '') {
            $('#txtPassword').focus();
            $('#txtPassword').parent().parent('div').find('.errMsg').empty();
            $('#txtPassword').parent().parent('div').append("<div class='errMsg' style='color:#cc0000;margin-top:5px;'><i>Password is required.</i></div>");
            return false;
        } else if (password != confirmPassword) {
            $('#txtConfirmPassword').focus();
            $('#txtConfirmPassword').parent().parent('div').find('.errMsg').empty();
            $('#txtConfirmPassword').parent().parent('div').append("<div class='errMsg' style='color:#cc0000;margin-top:5px;'><i>Password and confirm password do not match.</i></div>");
            return false;
        }
        else if (mobileNo == null || mobileNo == '') {
            $('#mobileNo').focus();
            $('#mobileNo').parent().parent('div').find('.errMsg').empty();
            $('#mobileNo').parent().parent('div').append("<div class='errMsg' style='color:#cc0000;margin-top:5px;'><i>MobileNo is required.</i></div>");
            return false;
        } else {
            return true;
        }
    }

    function validateSecondStep() {
        $('.errMsg').hide();
        var designationId = $('#designationId').val();
        var dentalSchoolId = $('#dentalSchoolId').val();
        //alert(dentalSchoolId);
        if (designationId == null || designationId == '' || designationId == 0) {
            
            $('#designationId').parent().parent('div').find('.errMsg').empty();
            $('#designationId').parent().parent('div').append("<div class='errMsg' style='color:#cc0000;margin-top:5px;'><i>Designation is required.</i></div>");
            return false;
        } else if (dentalSchoolId == null || dentalSchoolId == '' || dentalSchoolId == 0) {

            $('#dentalSchoolId').parent().parent('div').parent('div').find('.errMsg').empty();
            $('#dentalSchoolId').parent().parent('div').parent('div').append("<div class='errMsg' style='color:#cc0000;margin-top:5px;'><i>Dental School is required.</i></div>");
            return false;
        } else {
            return true;
        }
    }

    function validateThirdStep() {
        $('.errMsg').hide();
        var clinicName = $('#txtClinicName').val();
        var clinicAddress = $('#txtClinicAddress').val();

        if (clinicName == null || clinicName == '' ) {
            $('#txtClinicName').focus();
            $('#txtClinicName').parent().parent('div').find('.errMsg').empty();
            $('#txtClinicName').parent().parent('div').append("<div class='errMsg' style='color:#cc0000;margin-top:5px;'><i>Clinic Name is required.</i></div>");
            return false;
        } else if (clinicAddress == null || clinicAddress == '') {
            $('#txtClinicAddress').focus();
            $('#txtClinicAddress').parent().parent('div').find('.errMsg').empty();
            $('#txtClinicAddress').parent().parent('div').append("<div class='errMsg' style='color:#cc0000;margin-top:5px;'><i>Clinic Address  is required.</i></div>");
            return false;
        } else {
            return true;
        }
    }
	
	$("body").on("click", ".a-prevent", function(e) {
            e.preventDefault();
        })
 
})(jQuery); 