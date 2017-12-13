import {
    GET_NEWS_LIST,
    GET_NEWS_LIST_FAILURE,
    GET_NEWS_LIST_SUCCESS,
    getNewsSuccess,
    getNewsFailure
} from './newsActions';

import API from '../api';

import 'rxjs';
import {
    Observable
} from 'rxjs/Observable'

const fetchNewsEpic = action$ =>
    action$.ofType(GET_NEWS_LIST)
    .mergeMap(action => 
        Observable.fromPromise(API.getNews())
        .map(response => getNewsSuccess(response.data))
        .catch(error => Observable.of(getNewsFailure(error)))
    )

export default fetchNewsEpic;