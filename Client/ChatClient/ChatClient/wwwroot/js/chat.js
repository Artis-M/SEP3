function scroll(elementID) {
    let elem = document.getElementById(elementID);
    // elem.scrollTop = elem.scrollHeight;
    elem.scrollBy({
        top: elem.scrollHeight,
        left: 0,
        behavior: "smooth"
    });
}
function scrollMessageFragment(className) {
    let elem = document.getElementsByClassName(className);
    // elem.scrollTop = elem.scrollHeight;
    for(let i = 0; i < elem.length; i++){
        console.log("??????");
        console.log(i.toString());
        elem[i].scrollBy({
            left: elem[i].scrollWidth,
            behavior: "smooth"
        });
    }
}
