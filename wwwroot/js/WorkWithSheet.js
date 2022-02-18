function addDetailInfo(event) {

    let parent = event.target.parentElement; //document.getElementsByClassName("detailsInfoClass");
    ///alert(parent);
    let text = parent.previousSibling;
    let detailsInfo = text.previousSibling;

    let cloneNode = detailsInfo.cloneNode(true);
    let children = cloneNode.children;
    //alert(children.length);
    for (let i = 0; i < children.length; i++) {
        let subChildren = children[i].children;
        //alert("length " + subChildren.length);
        for (let j = 0; j < subChildren.length; j++) {
            //alert(subChildren[j].type);

            if (subChildren[j].type === "number") {
                //alert(subChildren[i].value);
                subChildren[j].value = "";

                let name = subChildren[j].name;
                let id = name[name.length - 1];
                id++;
                name = name.substr(0, name.length - 1) + id;
                subChildren[j].name = name;
                subChildren[j].value = 0;
                //let id = name[12];
                //id++;

                //name = name.substr(0, 12) + id + name.substr(13, 19);
                ////alert("Name - " + name);
                //subChildren[j].name = name;
                ////alert(subChildren[j].name + ", " + j);
                ////alert("length " + subChildren.length);
            }
        }
    }
    let nodeId = cloneNode.id;
    let id = nodeId[nodeId.length - 1];
    id++;
    //alert(nodeId.substr(0, nodeId.length - 1));
    cloneNode.id = nodeId.substr(0, nodeId.length - 1) + id;
    //alert(cloneNode.id);
    document.getElementById("detailsContainer").insertBefore(cloneNode, detailsInfo.nextSibling);
    //alert("Done");
}

async function detailChanged(event) {

    let form = document.sheetForm;
    let sheetWidth = form.elements.sheetWidth;
    let sheetHeight = form.elements.sheetHeight;

    //let detailWidth = form.elements.detailWidth;
    //let detailHeight = form.elements.detailHeight;
    //let detailCount = form.elements.detailCount;

    let sheetObj = {
        width: sheetWidth.value,
        height: sheetHeight.value
    };

    let detailsInfoObj = [];

    for (let i = 0; ; i++) {
        let detailWidth = form.elements["detailWidth" + i];
        let detailHeight = form.elements["detailHeight" + i];
        let detailCount = form.elements["detailCount" + i];

        if (detailWidth === undefined) {
            break;
        }

        detailsInfoObj[i] = {
            width: detailWidth.value,
            height: detailHeight.value,
            count: detailCount.value
        };
    }

    //detailsInfoObj[0] = {
    //    width: detailWidth.value,
    //    height: detailHeight.value,
    //    count: detailCount.value
    //};

    //for (let i = 1; i < detailWidth.length; i++) {
    //    detailsInfoObj[i] = {
    //        width: detailWidth.value,
    //        height: detailHeight.value,
    //        count: detailCount.value
    //    };
    //}

    let indexObj = {
        sheet: sheetObj,
        detailsInfo: detailsInfoObj,
        cuttedDetails: null
    }

    let response = await fetch('/home/fetch', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(indexObj)
    });

    //alert(sheetWidth.value + " " + sheetHeight.value + " " + detailWidth.value + " " + detailHeight.value + " " + detailCount.length);

}