import {
    GET_FILES,
    getFilesSuccess,
    getFilesFailure
} from './fileActions';

import API from '../api';

import 'rxjs';
import {
    Observable
} from 'rxjs/Observable'

const fetchFilesEpic = action$ =>
    action$.ofType(GET_FILES)
    .mergeMap(action =>
        Observable.fromPromise(API.getFiles(action.payload))
        .map(response => getFilesSuccess(response.data))
        .catch(error => Observable.of(getFilesFailure(error)))
    )

export default fetchFilesEpic;