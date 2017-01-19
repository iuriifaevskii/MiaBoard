/******************/
/* DrugAndDrop    */
/******************/

$(function () {

    $('#dashletHtml').draggable({
        connectToSortable: '.boardWorkspaceContainer',
        helper: "clone"
    });
    $('#dashletChart').draggable({
        connectToSortable: '.boardWorkspaceContainer',
        helper: "clone"
    });
    $('.boardWorkspaceContainer').sortable();

    $('.col-Container').sortable({
        connectWith: '.col-Container'
    });

    $('.col-Container').sortable({
        placeholder: 'emptySpace'
    });


});

/**********************/
/* End DrugAndDrop    */
/**********************/