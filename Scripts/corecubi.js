function loading() {
    console.log("cargando...")
    let divspinner = document.getElementById("divspinner")
    let fileinput = document.getElementById("fileinput")

    fileinput.setAttribute("style", "display:none")
    divspinner.setAttribute("style", "display:block")
}



$(document).ready(function () {



    let spinerload = document.getElementById("spinerload")
    let containertab = document.getElementById("containertab")
    console.log(spinerload)
    console.log(containertab)


    spinerload.setAttribute("style", "display:none")
    containertab.setAttribute("style", "display:block")

    $("#example").DataTable();

    console.log("ready")
    setTimeout(function () {
        alert('page is loaded and 1 minute has passed');
        //$("#datatable").DataTable({});


        console.log("ready")

    }, 6000);



});