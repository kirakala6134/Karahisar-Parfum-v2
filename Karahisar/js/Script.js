function degis(a) {
    document.getElementById("urunresim").src = a.src;
}
function changeAdet(miktar) {
    var el = document.getElementById('adetSayisi');
    var mevcutAdet = parseInt(el.innerText);
    var yeniAdet = mevcutAdet + miktar;
    if (yeniAdet < 1) yeniAdet = 1;
    if (yeniAdet > 10) yeniAdet = 10;
    el.innerText = yeniAdet;
}
function favori(fav) {
    fav.classList.toggle('fa-regular');
    fav.classList.toggle('fa-solid');
}
