import { HttpHeaders } from "@angular/common/http";

export class Helpers{
  public static loadToken() {
    let token = JSON.parse(localStorage.getItem('user'));
    if(token === null){
      return null;
    }
    let headerOptions = new HttpHeaders().set("Authorization", "Bearer " + token['token']);
    return headerOptions;
  }
}