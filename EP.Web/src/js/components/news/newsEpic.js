import { combineEpics } from 'redux-observable';
import 'rxjs';
import { Observable } from 'rxjs/Observable';

import {
    GET_NEWS_LIST,
    CREATE_NEWS
} from './types';

import {
    getNewsSuccess,
    getNewsFailure,
    createNewsSuccess,
    createNewsFailure
} from './newsActions';

import API from '../api';

const fetchNewsEpic = (action$, store) =>
    action$.ofType(GET_NEWS_LIST)
    .mergeMap((action) =>
        Observable.fromPromise(API.getNews(action.payload.page, action.payload.size))
        .map(response => getNewsSuccess(response.data))
        .catch(error => Observable.of(getNewsFailure(error)))
    )

const createNewsEpic = (action$, store) =>
    action$.ofType(CREATE_NEWS)
    .mergeMap((action) =>
        Observable.fromPromise(API.createNews(action.payload))
        .map(response => createNewsSuccess(response.data))
        .catch(error => Observable.of(createNewsFailure(error)))
    )

const epics = [
    fetchNewsEpic,
    createNewsEpic
];

const newsEpic = combineEpics(...Object.values(epics));

export default newsEpic;