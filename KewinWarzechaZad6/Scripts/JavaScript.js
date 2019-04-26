$("#del").click(function AreYouSure() {
    console.log("aabb");
    var r = confirm("Czy na pewno chcesz usunąć to ogłoszenie?");
    if (r == true) {
        return true;
    } else {
        return false;
    }
})
console.log('aa');