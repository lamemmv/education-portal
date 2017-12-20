
import { combineEpics } from 'redux-observable';

import {
    GET_NEWS_LIST,
    GOTO_CREATE_NEWS
} from './types';

import {
    getNewsSuccess,
    getNewsFailure,
    initCreateNews
} from './newsActions';

import API from '../api';
import 'rxjs';
import { Observable } from 'rxjs/Observable';
import { Epic } from 'redux-observable';

const fetchNewsEpic = (action$, store) =>
    action$.ofType(GET_NEWS_LIST)
        .mergeMap((action) =>
            Observable.fromPromise(API.getNews(action.payload.page))
                .map(response => getNewsSuccess(response.data))
                .catch(error => Observable.of(getNewsFailure(error)))
        )

const gotoCreateNewsEpic = (action$, store) =>
    action$.ofType(GOTO_CREATE_NEWS)
        .map(() => initCreateNews())

const epics = [
    fetchNewsEpic,
    gotoCreateNewsEpic
];

const newsEpic = combineEpics(...Object.values(epics));

export default newsEpic;