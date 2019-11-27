import store from '@/store'

export function getBtnCtr(path, dt,userBtn) {
  var btns = [];
  var findBtns = (path, dt) => {
    var data = dt
    for (var i = 0; i < data.length; i++) {
      if (path == data[i].path) {
        btns = data[i].buttons
      }
      if (data[i].children && data[i].children.length > 0) {
        findBtns(path, data[i].children)
      }
    }
  }
  findBtns(path, dt)
  var btnData = {}
  if(userBtn){
     var userButtons = userBtn;
  }else{
    var userButtons = store.getters.userButtons;
  }
  // userButtons.map((item) => {
  //   btnData[item.buttonKey.toLowerCase()] = {}
  //   console.log(1)
  // })
  for (var i = 0; i < userButtons.length; i++) {
    btnData[userButtons[i].buttonKey.toLowerCase()] = {}
  }
  
  btns.forEach(item => {
    item.buttonKey = item.buttonKey.toLowerCase()
    // console.log(1)
    for (var i in btnData) {
      // console.log(2)
      if (item.buttonKey == i) {
        btnData[i] = item;
      }
    }
  });


  return btnData;
}

export function getName(path, dt) {
  var btns = {};
  var findBtns = (path, dt) => {
    var data = dt
    for (var i = 0; i < data.length; i++) {
      if (path == data[i].path) {
        btns = data[i]
      }
      if (data[i].children && data[i].children.length > 0) {
        findBtns(path, data[i].children)
      }
    }
  }
  findBtns(path, dt)

  return btns;
}

export function getPath(name, dt) {
  var btns = {};
  var findBtns = (name, dt) => {
    var data = dt
    for (var i = 0; i < data.length; i++) {
      if (name == data[i].name) {
        btns = data[i]
      }
      if (data[i].children && data[i].children.length > 0) {
        findBtns(name, data[i].children)
      }
    }
  }
  findBtns(name, dt)

  return btns;
}
