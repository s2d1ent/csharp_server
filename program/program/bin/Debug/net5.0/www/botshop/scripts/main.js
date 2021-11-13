var doc=document,
    wiw=window,
    body = $("body");
var inputScripts = [
    "*/scripts/main__firstlvl.js"
    ];

  for (var i=0;i<inputScripts.length;i++) {
    let change;
    if (inputScripts[i].includes("*")==true) {
      let script = doc.createElement('script');
      change=inputScripts[i].replace("*","https://lazycote.github.io");
      script.src=change;
      body.append(script);
    } else {
      let script = doc.createElement('script');
      change=inputScripts[i];
      script.src=change;
      body.append(script);
    }
  }

