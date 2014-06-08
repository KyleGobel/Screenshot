var page = require('webpage').create();
page.open(#URL#, function() {
  page.render(#SAVE_PATH#);
  phantom.exit();
});