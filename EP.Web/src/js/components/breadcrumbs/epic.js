import {
    combineEpics
} from 'redux-observable';
import {
    Observable
} from 'rxjs/Observable';
import 'rxjs';

import {
    GOTO_NEWS_HOME,
    GOTO_FILES,
    GOTO_HOME,
    GOTO_NEWS_DETAIL
} from './types';

import { GOTO_CREATE_NEWS } from '../news/types';

import {
    gotoFiles,
    gotoHomeNews,
    gotoNewsDetail,
    handleHomeRoute,
    handleFilesRoute,
    handleNewsHomeRoute,
    handleNewsCreateRoute,
    handleNewsDetailRoute
} from './actions';

const gotoHomeEpic = (action$, store) =>
    action$.ofType(GOTO_HOME)
    .map(action => handleHomeRoute(action.payload));

const gotoNewsHomeEpic = (action$, store) =>
    action$.ofType(GOTO_NEWS_HOME)
    .flatMap(action =>
        Observable.of(
            handleHomeRoute(),
            handleNewsHomeRoute()
        )
    );

const gotoNewsCreateEpic = (action$, store) =>
    action$.ofType(GOTO_CREATE_NEWS)
    .flatMap(action =>
        Observable.of(
            handleHomeRoute(),
            handleNewsHomeRoute(),
            handleNewsCreateRoute()
        )
    );

const gotoNewsDetailEpic = (action$, store) =>
    action$.ofType(GOTO_NEWS_DETAIL)
    .flatMap(action =>
        Observable.of(
            handleHomeRoute(),
            handleNewsHomeRoute(),
            handleNewsDetailRoute(action.payload)
        )
    );

const gotoFilesEpic = (action$, store) =>
    action$.ofType(GOTO_FILES)
    .flatMap(action =>
        Observable.of(
            handleHomeRoute(),
            handleFilesRoute()
        )
    );

const epics = [
    gotoNewsHomeEpic,
    gotoNewsCreateEpic,
    gotoNewsDetailEpic,
    gotoFilesEpic,
    gotoHomeEpic
];

const breadcrumbsEpic = combineEpics(...Object.values(epics));

export default breadcrumbsEpic;