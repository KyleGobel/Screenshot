var page = require('webpage').create();
page.open("http://www.gloucesterva.info/MuseumofHistory/tabid/1033/Default.aspx", function() {
  page.render("screenshots/9f21da06bb394de7bcac892ece2f78cf.png");
  phantom.exit();
});