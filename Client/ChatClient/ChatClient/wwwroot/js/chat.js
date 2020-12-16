function scroll(elementID) {
    let elem = document.getElementById(elementID);
    elem.scrollBy({
        top: elem.scrollHeight,
        left: 0,
        behavior: "smooth"
    });
}
function scrollMessageFragment(className) {
    let elem = document.getElementsByClassName(className);
    for(let i = 0; i < elem.length; i++){
        console.log("??????");
        console.log(i.toString());
        elem[i].scrollBy({
            left: elem[i].scrollWidth,
            behavior: "smooth"
        });
    }
}
function confirmationPromt(message){
   let response = confirm(message);
   return response;
}
