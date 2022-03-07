
//let isValid = true;

//var container = document.querySelector('#container');
//var msnry = new Masonry(container, {
//    // Настройки
//    //columnWidth: 200,
//    itemSelector: '.item'
//});

//$('.grid').masonry({
//    itemSelector: '.grid-item',
//    columnWidth: '.grid-sizer',
//    gutter: 0,
//    //horizontalOrder: true,
//    percentPosition: true,
//});

let colors = [
    "blue",
    "yellow",
    "red",
    "green",
    "violet",
    "brown",
    "orange"
];


window.onload = function () {
    document.getElementsByName("removeDetailInfo")[0].classList.add("disabled");
}


function addDetailInfo(event) {

    if (document.getElementsByName("detailCount").length == 1) {
        document.getElementsByName("removeDetailInfo")[0].classList.remove("disabled");
    }

    let parent = event.target.parentElement; //document.getElementsByClassName("detailsInfoClass");
    ///console.log(parent);
    let text = parent.previousSibling;
    let detailsInfo = text.previousSibling;

    let cloneNode = detailsInfo.cloneNode(true);
    let children = cloneNode.children;
    //console.log(children.length);

    for (let i = 0; i < children.length; i++) {
        let subChildren = children[i].children;
        //console.log("length " + subChildren.length);
        for (let j = 0; j < subChildren.length; j++) {
            //console.log(subChildren[j].type);

            if (subChildren[j].type === "number") {
                //console.log(subChildren[i].value);
                if (subChildren[j].name === "detailCount") {
                    subChildren[j].value = 0;
                    subChildren[j].classList.remove("border-warning");
                    subChildren[j].classList.remove("border-3");
                    subChildren[j].nextElementSibling.innerText = "";
                }
                else {
                    subChildren[j].value = 50;
                    subChildren[j].classList.remove("border-danger");
                    subChildren[j].classList.remove("border-3");
                    subChildren[j].nextElementSibling.innerText = "";
                }
            }
        }
    }

    let nodeId = cloneNode.id;
    let id = nodeId[nodeId.length - 1];
    id++;
    //console.log(nodeId.substr(0, nodeId.length - 1));
    cloneNode.id = nodeId.substr(0, nodeId.length - 1) + id;
    //console.log(cloneNode.id);
    document.getElementById("detailsContainer").insertBefore(cloneNode, detailsInfo.nextSibling);
    //console.log("Done");

    let deleteButtons = document.getElementsByName("removeDetailInfo");
    let addedButton = deleteButtons[deleteButtons.length - 1];

    let arrayColorsOfButtons = [];
    for (let i = 0; i < deleteButtons.length; i++) {
        arrayColorsOfButtons.push(deleteButtons[i].style.backgroundColor);
    }

    for (let i = 0, j = 0; i < colors.length; i++) {
        if (arrayColorsOfButtons.includes(colors[i])) {
            continue;
        }
        addedButton.style.backgroundColor = colors[i];
        break;
    }
    //deleteButtons[deleteButtons.length - 1].style.backgroundColor = colors[deleteButtons.length];

    //updateDetailsPartial();
}


 function detailOnInput(event) {

    updateDetailsPartial();
}


function removeDetailInfo(event) {
    let parent = event.target.parentElement; 
    console.log(parent);
    let detailsInfo = parent.parentElement;
    console.log(detailsInfo);

    detailsInfo.remove();

    if (document.getElementsByName("detailCount").length == 1) {
        document.getElementsByName("removeDetailInfo")[0].classList.add("disabled");
    }

    updateDetailsPartial();
}


async function updateDetailsPartial() {

    //isValid = true;
    let sheetWidth = document.getElementById("sheetWidthId");
    let sheetHeight = document.getElementById("sheetHeightId");

    validate(
        sheetWidth,
        500,
        1500,
        "border-danger",
        "border-3",
        document.getElementById("sheetWidthValidationId"),
        "Width must be not less than 0 and most than 1500");

    validate(
        sheetHeight,
        500,
        1500,
        "border-danger",
        "border-3",
        document.getElementById("sheetHeightValidationId"),
        "Height must be not less than 0 and most than 1500");

    let sheetObj = {
        width: sheetWidth.value === "" ? 0 : sheetWidth.value,
        height: sheetHeight.value === "" ? 0 : sheetHeight.value
    };


    let detailWidthes = document.getElementsByName("detailWidth");
    let detailHeights = document.getElementsByName("detailHeight");
    let detailCounts = document.getElementsByName("detailCount");

    let detailWidthValidations = document.getElementsByName("detailWidthValidation");
    let detailHeightValidations = document.getElementsByName("detailHeightValidation");
    let detailCountValidations = document.getElementsByName("detailCountValidation");
    let deleteButtons = document.getElementsByName("removeDetailInfo");

    let arrayColorsOfButtons = [];
    for (let i = 0; i < deleteButtons.length; i++) {
        arrayColorsOfButtons.push(deleteButtons[i].style.backgroundColor);
    }

    let detailsInfoObj = [];

    for (let i = 0; i < detailCounts.length; i++) {

        detailsInfoObj[i] = {
            width: detailWidthes[i].value === "" ? 0 : detailWidthes[i].value,
            height: detailHeights[i].value === "" ? 0 : detailHeights[i].value,
            count: detailCounts[i].value === "" ? 0 : detailCounts[i].value,
            backgroundColor: colors.indexOf(deleteButtons[i].style.backgroundColor) //deleteButtons[i].style.backgroundColor
        };

        validate(
            detailWidthes[i],
            50,
            Infinity,
            "border-danger",
            "border-3",
            detailWidthValidations[i],
            "Width must be not less than 50");

        validate(detailHeights[i],
            50,
            Infinity,
            "border-danger",
            "border-3",
            detailHeightValidations[i],
            "Height must be not less than 50");

        validate(detailCounts[i],
            1,
            Infinity,
            "border-warning",
            "border-bottom",
            detailCountValidations[i],
            "");
    }

    //if (!isValid) {
    //    return;
    //}

    let indexObj = {
        sheet: sheetObj,
        detailsInfo: detailsInfoObj,
        cuttedDetails: null
    }

    let response = await fetch('/Home/FetchDetailsPartial', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(indexObj)
    });

    let result = await response.text();
    document.getElementById("detailsPartialId").innerHTML = result;

    for (var i = 0; i < deleteButtons.length; i++) {
        deleteButtons[i].style.backgroundColor = detailsInfoObj[i].backgroundColor; //colors[i];
    }

    // masonry ////////////////////
    $('.grid').masonry({
        itemSelector: '.grid-item',
        columnWidth: '.grid-sizer',
        horizontalOrder: true,
        // gutter: 0,
        // percentPosition: true,
        resize: false
    });
    // ////////////////////////////

    ///////////////////////////////////////////////////////////////////
    let isOverflow = checkOverflow(document.getElementById("sheetId"));
    let sheetEl = document.getElementById("sheetId")
    let overflowEl = document.getElementById("overflowId");
    if (isOverflow) {
        overflowEl.hidden = false;
        if (!sheetEl.classList.contains("opacity-50")) {
            sheetEl.classList.add("opacity-50");
        }
    }
    else {
        isOverflow.hidden = true;
        sheetEl.classList.remove("opacity-50");
    }
    //////////////////////////////////////////////////////////////////
}


function validate(element, minValue, maxValue, className1, className2, validationElement, validationText) {
    if (element.value < minValue || element.value > maxValue) {
        if (!element.classList.contains(className1))
            element.classList.add(className1);
        if (!element.classList.contains(className2))
            element.classList.add(className2);
        validationElement.innerText = validationText;
        //isValid = false;
    }
    else {
        element.classList.remove(className1);
        element.classList.remove(className2);
        validationElement.innerText = "";
    }
}


function checkOverflow(el) {
    let curOverflow = el.style.overflow;

    if (!curOverflow || curOverflow === "visible")
        el.style.overflow = "hidden";

    let isOverflowing = el.clientWidth < el.scrollWidth
        || el.clientHeight < el.scrollHeight;

    el.style.overflow = curOverflow;

    return isOverflowing;
}