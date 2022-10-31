import { ErrorHandler, Inject } from '@angular/core';


export class CustomErrorHandler implements ErrorHandler {

    constructor() {
    }

    handleError(error: any): void {
        console.log(error);
        this.showAlert(error);
    }

    private showAlert(error: any) :void {
        if (console && console.group && console.error && error.error && error.error.messages) {
            alert(error.error.messages[0]);
        }
    }
}
