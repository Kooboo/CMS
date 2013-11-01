/*
*   settings
*   author: ronglin
*   create date: 2011.12.28
*/

(function (win) {

    // root namespane
    var ctx = {};

    // register
    win.visualstyle = ctx;

    // global appSetting
    ctx.proactiveSaving = true;
    ctx.selectorEditable = true;

    // css prefix ignore
    ctx.ignorePrefixs = ['-ms-', '-moz-', '-webkit-', '-o-'];

    var empty = {}, dft = '';

    var fontFamily = {
        "Arial, Helvetica, sans-serif": dft,
        "'Arial Black', Gadget, sans-serif": dft,
        "'Bookman Old Style', serif": dft,
        "'Comic Sans MS', cursive": dft,
        "Courier, monospace": dft,
        "'Courier New', Courier, monospace": dft,
        "Garamond, serif": dft,
        "Georgia, serif": dft,
        "Impact, Charcoal, sans-serif": dft,
        "'Lucida Console', Monaco, monospace": dft,
        "'Lucida Sans Unicode', 'Lucida Grande', sans-serif": dft,
        "'MS Sans Serif', Geneva, sans-serif": dft,
        "'MS Serif', 'New York', sans-serif": dft,
        "'Palatino Linotype', 'Book Antiqua', Palatino, serif": dft,
        "Symbol, sans-serif": dft,
        "Tahoma, Geneva, sans-serif": dft,
        "'Times New Roman', Times, serif": dft,
        "'Trebuchet MS', Helvetica, sans-serif": dft,
        "Verdana, Geneva, sans-serif": dft,
        "Webdings, sans-serif": dft,
        "Wingdings, 'Zapf Dingbats', sans-serif": dft
    };

    var nameColors = {
        'inherit': dft,
        'transparent': dft,
        'Aqua': dft,
        'Black': dft,
        'Blue': dft,
        'Fuchsia': dft,
        'Gray': dft,
        'Green': dft,
        'Maroon': dft,
        'Navy': dft,
        'Olive': dft,
        'Purple': dft,
        'Red': dft,
        'Silver': dft,
        'Teal': dft,
        'White': dft,
        'Yellow': dft,
        'ActiveBorder': dft,
        'ActiveCaption': dft,
        'AppWorkspace': dft,
        'Background': dft,
        'ButtonFace': dft,
        'ButtonHighlight': dft,
        'ButtonShadow': dft,
        'ButtonText': dft,
        'CaptionText': dft,
        'GrayText': dft,
        'Highlight': dft,
        'HighlightText': dft,
        'InactiveBorder': dft,
        'InactiveCaption': dft,
        'InactiveCaptionText': dft,
        'InfoBackground': dft,
        'InfoText': dft,
        'Menu': dft,
        'MenuText': dft,
        'Scrollbar': dft,
        'ThreeDDarkShadow': dft,
        'ThreeDFace': dft,
        'ThreeDHighlight': dft,
        'ThreeDLighShadow': dft,
        'ThreeDShadow': dft,
        'Window': dft,
        'WindowFrame': dft,
        'WindowText': dft
    };

    var lineStyles = {
        'dashed': dft,
        'dotted': dft,
        'double': dft,
        'groove': dft,
        'hidden': dft,
        'inset': dft,
        'none': dft,
        'outset': dft,
        'ridge': dft,
        'solid': dft,
        'inherit': dft
    };

    var overflows = {
        'auto': dft,
        'hidden': dft,
        'scroll': dft,
        'visible': dft,
        'inherit': dft
    };

    ctx.cssLengthUnits = {
        'px': dft,
        '%': dft,
        'pt': dft,
        'em': dft,
        'ex': dft,
        'pc': dft,
        'mm': dft,
        'cm': dft,
        'in': dft
    };

    ctx.propertySet = {
        'azimuth': {
            'behind': dft,
            'center': dft,
            'center-left': dft,
            'center-right': dft,
            'far-left': dft,
            'far-right': dft,
            'left': dft,
            'left-side': dft,
            'leftwards': dft,
            'right': dft,
            'right-side': dft,
            'rightwards': dft,
            'inherit': dft
        },
        'background': empty,
        'background-color': nameColors,
        'background-image': empty,
        'background-repeat': {
            'no-repeat': dft,
            'repeat': dft,
            'repeat-x': dft,
            'repeat-y': dft,
            'inherit': dft
        },
        'background-position': {
            'bottom': dft,
            'center': dft,
            'left': dft,
            'right': dft,
            'top': dft,
            'inherit': dft
        },
        'background-attachment': {
            'fixed': dft,
            'scroll': dft,
            'inherit': dft
        },

        'border': empty,
        'border-spacing': empty,
        'border-collapse': {
            'inherit': dft,
            'collapse': dft,
            'separate': dft
        },
        'border-color': nameColors,
        'border-width': empty,
        'border-style': lineStyles,

        'border-left': empty,
        'border-left-color': nameColors,
        'border-left-width': empty,
        'border-left-style': lineStyles,

        'border-top': empty,
        'border-top-color': nameColors,
        'border-top-width': empty,
        'border-top-style': lineStyles,

        'border-right': empty,
        'border-right-color': nameColors,
        'border-right-width': empty,
        'border-right-style': lineStyles,

        'border-bottom': empty,
        'border-bottom-color': nameColors,
        'border-bottom-width': empty,
        'border-bottom-style': lineStyles,

        'outline': empty,
        'outline-color': nameColors,
        'outline-width': empty,
        'outline-style': lineStyles,

        'caption-side': {
            'bottom': dft,
            'left': dft,
            'right': dft,
            'top': dft,
            'inherit': dft
        },

        'clear': {
            'both': dft,
            'left': dft,
            'none': dft,
            'right': dft,
            'inherit': dft
        },

        'color': nameColors,

        'content': {
            'close-quote': dft,
            'no-close-quote': dft,
            'no-open-quote': dft,
            'open-quote': dft,
            'inherit': dft
        },

        'counter-increment': {
            'none': dft,
            'inherit': dft
        },

        'counter-reset': {
            'none': dft,
            'inherit': dft
        },

        'cursor': {
            'auto': dft,
            'crosshair': dft,
            'default': dft,
            'e-resize': dft,
            'help': dft,
            'move': dft,
            'n-resize': dft,
            'ne-resize': dft,
            'nw-resize': dft,
            'pointer': dft,
            's-resize': dft,
            'se-resize': dft,
            'sw-resize': dft,
            'text': dft,
            'w-resize': dft,
            'wait': dft,
            'inherit': dft
        },

        'direction': {
            'ltr': dft,
            'rtl': dft,
            'inherit': dft
        },

        'display': {
            'block': dft,
            'inherit': dft,
            'inline': dft,
            'inline-block': dft,
            'inline-table': dft,
            'list-item': dft,
            'none': dft,
            'run-in': dft,
            'table': dft,
            'table-caption': dft,
            'table-cell': dft,
            'table-column': dft,
            'table-column-group': dft,
            'table-row': dft,
            'table-row-group': dft,
            'table-footer-group': dft,
            'table-header-group': dft
        },

        'elevation': {
            'above': dft,
            'below': dft,
            'higher': dft,
            'level': dft,
            'lower': dft,
            'inherit': dft
        },

        'empty-cells': {
            'hide': dft,
            'show': dft,
            'inherit': dft
        },

        'float': {
            'left': dft,
            'none': dft,
            'right': dft,
            'inherit': dft
        },

        'font': empty,
        'font-family': fontFamily,
        'font-size-adjust': empty,
        'font-size': {
            'large': dft,
            'larger': dft,
            'medium': dft,
            'small': dft,
            'smaller': dft,
            'x-large': dft,
            'x-small': dft,
            'xx-large': dft,
            'xx-small': dft
        },
        'font-weight': {
            'bold': dft,
            'bolder': dft,
            'lighter': dft,
            'normal': dft,
            '100': dft,
            '200': dft,
            'inherit': dft
        },
        'font-stretch': {
            'inherit': dft,
            'normal': dft,
            'wider': dft,
            'narrower': dft,
            'ultra-condensed': dft,
            'extra-condensed': dft,
            'condensed': dft,
            'semi-condensed': dft,
            'semi-expanded': dft,
            'expanded': dft,
            'extra-expanded': dft,
            'ultra-expanded': dft
        },
        'font-variant': {
            'normal': dft,
            'inherit': dft,
            'small-caps': dft
        },
        'font-style': {
            'inherit': dft,
            'italic': dft,
            'normal': dft,
            'oblique': dft
        },

        'list-style': empty,
        'list-style-image': empty,
        'list-style-position': {
            'inside': dft,
            'outside': dft,
            'inherit': dft
        },
        'list-style-type': {
            'armenian': dft,
            'circle': dft,
            'cjk-ideographic': dft,
            'decimal': dft,
            'decimal-leading-zero': dft,
            'disc': dft,
            'georgian': dft,
            'hebrew': dft,
            'hiragana': dft,
            'hiragana-iroha': dft,
            'katakana': dft,
            'katakana-iroha': dft,
            'lower-alpha': dft,
            'lower-greek': dft,
            'lower-latin': dft,
            'lower-roman': dft,
            'none': dft,
            'square': dft,
            'upper-alpha': dft,
            'upper-latin': dft,
            'upper-roman': dft,
            'inherit': dft
        },

        'margin': empty,
        'margin-bottom': empty,
        'margin-left': empty,
        'margin-right': empty,
        'margin-top': empty,

        'marks': {
            'crop': dft,
            'cross': dft,
            'none': dft,
            'inherit': dft
        },

        'overflow': overflows,
        'overflow-x': overflows,
        'overflow-y': overflows,

        'padding': empty,
        'padding-bottom': empty,
        'padding-left': empty,
        'padding-right': empty,
        'padding-top': empty,

        'page-break-after': {
            'always': dft,
            'auto': dft,
            'avoid': dft,
            'left': dft,
            'right': dft,
            'inherit': dft
        },
        'page-break-before': {
            'always': dft,
            'auto': dft,
            'avoid': dft,
            'left': dft,
            'right': dft,
            'inherit': dft
        },
        'page-break-inside': {
            'auto': dft,
            'avoid': dft,
            'inherit': dft
        },

        'pause': empty,
        'pause-after': empty,
        'pause-before': empty,

        'pitch-range': empty,
        'pitch': {
            'high': dft,
            'low': dft,
            'medium': dft,
            'x-high': dft,
            'x-low': dft,
            'inherit': dft
        },

        'play-during': {
            'auto': dft,
            'mix': dft,
            'none': dft,
            'repeat': dft,
            'inherit': dft
        },

        'position': {
            'absolute': dft,
            'fixed': dft,
            'relative': dft,
            'static': dft,
            'inherit': dft
        },

        'size': {
            'auto': dft,
            'landscape': dft,
            'portrait': dft,
            'inherit': dft
        },

        'speak': {
            'none': dft,
            'normal': dft,
            'spell-out': dft,
            'inherit': dft
        },
        'speak-header': {
            'always': dft,
            'once': dft,
            'inherit': dft
        },
        'speak-numeral': {
            'continuous': dft,
            'digits': dft,
            'inherit': dft
        },
        'speak-punctuation': {
            'code': dft,
            'none': dft,
            'inherit': dft
        },
        'speech-rate': {
            'fast': dft,
            'faster': dft,
            'medium': dft,
            'slow': dft,
            'slower': dft,
            'x-fast': dft,
            'x-slow': dft,
            'inherit': dft
        },

        'table-layout': {
            'auto': dft,
            'fixed': dft,
            'inherit': dft
        },

        'text-decoration': {
            'none': dft,
            'underline': dft,
            'blink': dft,
            'overline': dft,
            'line-through': dft,
            'inherit': dft
        },

        'text-indent': empty,
        'text-shadow': empty,
        'text-align': {
            'center': dft,
            'justify': dft,
            'left': dft,
            'right': dft,
            'inherit': dft
        },

        'text-transform': {
            'capitalize': dft,
            'lowercase': dft,
            'none': dft,
            'uppercase': dft,
            'inherit': dft
        },

        'unicode-bidi': {
            'bidi-override': dft,
            'embed': dft,
            'normal': dft,
            'inherit': dft
        },

        'vertical-align': {
            'baseline': dft,
            'bottom': dft,
            'middle': dft,
            'sub': dft,
            'super': dft,
            'text-bottom': dft,
            'text-top': dft,
            'top': dft,
            'inherit': dft
        },

        'visibility': {
            'collapse': dft,
            'hidden': dft,
            'visible': dft,
            'inherit': dft
        },

        'volume': {
            'loud': dft,
            'medium': dft,
            'silent': dft,
            'soft': dft,
            'x-loud': dft,
            'x-soft': dft,
            'inherit': dft
        },

        'white-space': {
            'normal': dft,
            'nowrap': dft,
            'pre': dft,
            'inherit': dft
        },

        'word-spacing': {
            'normal': dft,
            'inherit': dft
        },

        'word-wrap': {
            'normal': dft,
            'break-word': dft,
            'inherit': dft
        },

        'left': {
            'auto': dft,
            'inherit': dft
        },
        'top': {
            'auto': dft,
            'inherit': dft
        },
        'right': {
            'auto': dft,
            'inherit': dft
        },
        'bottom': {
            'auto': dft,
            'inherit': dft
        },

        'width': empty,
        'height': empty,
        'max-width': empty,
        'max-height': empty,
        'min-width': empty,
        'min-height': empty,

        'clip': empty,
        'cue': empty,
        'cue-after': empty,
        'cue-before': empty,
        'marker-offset': empty,
        'orphans': empty,
        'quotes': empty,
        'richness': empty,
        'src': empty,
        'stress': empty,
        'voice-family': empty,
        'widows': empty,
        'letter-spacing': empty,
        'line-height': empty,
        'z-index': empty,

        'ime-mode': {
            'auto': dft,
            'normal': dft,
            'active': dft,
            'inactive': dft,
            'disabled': dft
        },

        'filter': empty,
        'opacity': empty,
        '-moz-opacity': empty,
        '-khtml-opacity': empty,

        'box-shadow': empty,
        '-moz-box-shadow': empty,
        '-webkit-box-shadow': empty,

        'border-radius': empty,
        'border-top-left-radius': empty,
        'border-top-right-radius': empty,
        'border-bottom-right-radius': empty,
        'border-bottom-left-radius': empty,

        '-moz-border-radius': empty,
        '-moz-border-radius-topleft': empty,
        '-moz-border-radius-topright': empty,
        '-moz-border-radius-bottomright': empty,
        '-moz-border-radius-bottomleft': empty,

        '-webkit-border-radius': empty,
        '-webkit-border-top-left-radius': empty,
        '-webkit-border-top-right-radius': empty,
        '-webkit-border-bottom-right-radius': empty,
        '-webkit-border-bottom-left-radius': empty
    };

} (window));
