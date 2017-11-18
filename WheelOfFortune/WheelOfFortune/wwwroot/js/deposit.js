$(document).ready(function () {

    $("#DepositForm").submit(function (e) {

        e.preventDefault();
        $(".alert-danger").addClass("hidden");
        $(".alert-success").hide("hidden");
        var button = $(this).find("button").button("loading");

        $.ajax({
            type: "POST",
            url: "Deposit/usecoupon",
            data: "vouchercode=" + $("#voucherCode").val(),
            success: function (data) {
                if (data.status == 1) {

                    $(".successMessage").html(data.message);
                    $(".alert-success").removeClass("hidden");
                    $("#voucherCode").val("");

                }
                else {
                    $(".errorMessage").html(data.message)
                    $(".alert-danger").removeClass("hidden");
                    
                }

                button.button("reset");
            },



        });

    });
});

function resetAlerts() {
    $(".alert-danger").addClass("hidden");
    $(".alert-success").hide("hidden");
}