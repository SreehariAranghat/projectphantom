
/*-----------------------------------------------------Global Keboard Short Cuts-------------------------------------------*/
var ksNewTicket = "Ctrl+Alt+T";

$(document).mapKey(ksNewTicket, function (e) { slider.slideReveal("toggle"); });

/*--------------------------------------------------------------------------------------------------------------------------*/
var slider;

$("#document").ready(function () {

   slider = $('#slider').slideReveal({

        position: "right"
        , push: false
        , overlay: true
        , width: 600
        , speed : 500
    });
});

/*------------------------------------------------------------Global Events------------------------------------------------------------------------*/
$("[showremoteui]").click(function () {
   slider.slideReveal("toggle");
});
