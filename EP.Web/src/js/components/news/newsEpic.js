import {
    combineEpics
} from 'redux-observable';
import 'rxjs';
import {
    Observable
} from 'rxjs/Observable';

import {
    GET_NEWS_LIST,
    CREATE_NEWS,
    CREATE_NEWS_SUCCESS
} from './types';

import {
    getNewsSuccess,
    getNewsFailure,
    createNewsSuccess,
    createNewsFailure
} from './newsActions';

import {
    addNotification
} from '../notify/notification.actions';

import API from '../api';
import Service from '../errorHandler';

const fetchNewsEpic = (action$, store) =>
    action$.ofType(GET_NEWS_LIST)
    .mergeMap((action) =>
        Observable.fromPromise(API.getNews(action.payload.page, action.payload.size))
        .map(response => getNewsSuccess(response.data))
        .catch(error => Observable.of(getNewsFailure(error)))
    );

const createNewsEpic = action$ =>
    action$.ofType(CREATE_NEWS)
    .mergeMap(action =>
        Observable.fromPromise(API.createNews(action.payload))
        .map(response => createNewsSuccess(response.data))
        .catch(error => Observable.of(createNewsFailure(error)))
    );

const createNewsSuccessEpic = action$ =>
    action$.ofType(CREATE_NEWS_SUCCESS)
    .flatMap(action => {
        return Observable.of(addNotification(Service.getMessage('succeed'), 'success', 'Create News'))
    });

const epics = [
    fetchNewsEpic,
    createNewsEpic,
    createNewsSuccessEpic
];

const newsEpic = combineEpics(...Object.values(epics));

export default newsEpic;