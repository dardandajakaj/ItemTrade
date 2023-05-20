// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  firebase: {
    projectId: 'diplomaapp-6bea8',
    appId: '1:104637841693:web:ca11bfee73f023dedfa1f4',
    apiKey: 'AIzaSyA4u5qBJ7pWXDkq--LxoBKFG39FZxSDgvw',
    authDomain: 'diplomaapp-6bea8.firebaseapp.com',
    messagingSenderId: '104637841693',
    storageBucket: 'gs://diplomaapp-6bea8.appspot.com'
  },
  production: false,
  url: "https://localhost:7103/api/",
  hub: "https://localhost:7103/chat",
  itemsPerPage: 5,
  pageNumber: 1
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
