export function closest(el, selector) {
    var matchesSelector =
        el.matches ||
        el.webkitMatchesSelector ||
        el.mozMatchesSelector ||
        el.msMatchesSelector;

      while (el) {
        if (matchesSelector.call(el, selector)) {
          break;
        }
        el = el.parentElement;
      }
      return el;
  }
  export function getStyle(dom, attr) {
    return dom.currentStyle
      ? dom.currentStyle[attr]
      : getComputedStyle(dom, false)[attr];
  };