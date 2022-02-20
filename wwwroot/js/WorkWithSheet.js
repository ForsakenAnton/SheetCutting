
//let isValid = true;


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

    validate(sheetWidth, 500, 1500, "border-danger", "border-3", document.getElementById("sheetWidthValidationId"), "Width must be not less than 0 and most than 1500");
    validate(sheetHeight, 500, 1500, "border-danger", "border-3", document.getElementById("sheetHeightValidationId"), "Height must be not less than 0 and most than 1500");

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

    let detailsInfoObj = [];

    for (let i = 0; i < detailCounts.length; i++) {

        detailsInfoObj[i] = {
            width: detailWidthes[i].value === "" ? 0 : detailWidthes[i].value,
            height: detailHeights[i].value === "" ? 0 : detailHeights[i].value,
            count: detailCounts[i].value === "" ? 0 : detailCounts[i].value
        };

        validate(detailWidthes[i], 50, Infinity, "border-danger", "border-3", detailWidthValidations[i], "Width must be not less than 50");
        validate(detailHeights[i], 50, Infinity, "border-danger", "border-3", detailHeightValidations[i], "Height must be not less than 50");
        validate(detailCounts[i], 1, Infinity, "border-warning", "border-bottom", detailCountValidations[i], "");
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

    //console.log(sheetWidth.value + " " + sheetHeight.value + " " + detailWidth.value + " " + detailHeight.value + " " + detailCount.length);
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