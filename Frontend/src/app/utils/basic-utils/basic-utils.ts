import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BasicUtils {

  constructor() { }

  stringDateConvert(stringDate: string) {
    var d = new Date(stringDate);
    let ye = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(d);
    let mo = new Intl.DateTimeFormat('en', { month: 'short' }).format(d);
    let da = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(d);
    return `${da}-${mo}-${ye}`
  }

}
