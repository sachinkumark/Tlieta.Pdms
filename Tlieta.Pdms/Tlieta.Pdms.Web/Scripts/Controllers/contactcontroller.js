﻿var app = angular.module('tlieta', ['kendo.directives']);
function contactcontroller($scope) {

    $scope.products = [
        { id: 1, name: 'Tennis Balls', department: 'Sports', lastShipment: '10/01/2013' },
        { id: 2, name: 'Basket Balls', department: 'Sports', lastShipment: '10/02/2013' },
        { id: 3, name: 'Oil', department: 'Auto', lastShipment: '10/01/2013' },
        { id: 4, name: 'Filters', department: 'Auto', lastShipment: '10/01/2013' },
        { id: 5, name: 'Dresser', department: 'Home Furnishings', lastShipment: '10/01/2013' }
    ];

    $scope.buttontext = "Add Contact";

    $scope.contact = { ContactId: 0, ContactName: "", Address: "", Mobile: "", Email: "", Company: "", Designation: "", UpdatedOn: new Date()
    }

    $scope.refreshcontact = function () {
        $scope.contact = {
            ContactId: 0, ContactName: "", Address: "", Mobile: "", Email: "", Company: "", Designation: "", UpdatedOn: new Date()
        };
        $scope.buttontext = "Add Contact";
    }

    $scope.savecontact = function (contact) {
        $.ajax({
            url: "/Contact/Save",
            async: false,
            type: "POST",
            data: JSON.stringify(contact),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (e) {
                RefreshGrid();
                $scope.refreshcontact();
            },
            error: function (e) {
                alert("Record could not be saved");
            }
        });
    }

    $scope.deletecontact = function (id) {
        var retVal = confirm('Are you sure you want to delete the contact?');
        if (retVal) {
            var url = '/Contact/Delete?id=' + id;
            $.ajax({
                url: url,
                type: 'POST',
                datatype: 'json',
                success: function (e) {
                    RefreshGrid();
                    $scope.refreshcontact();
                },
                error: function (error) {
                    alert("Record could not be deleted");
                }
            });
        }
    }
}


function Edit(e) {
    var dataItem = this.dataItem($(e.target).closest("tr"));
    var scope = angular.element("#ContactName").scope();
    scope.$apply(function () {
        scope.contact = dataItem;
        scope.buttontext = "Update Contact";
    });
    return false;
}

function Delete(e) {
    var dataItem = this.dataItem($(e.target).closest("tr"));
    angular.element("#ContactName").scope().deletecontact(dataItem.ContactId);
    return false;
}

function RefreshGrid() {
    var grid = $("#ContactGrid").data("kendoGrid");
    grid.dataSource.read();
}
