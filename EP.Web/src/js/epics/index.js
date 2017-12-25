import NewsEpic from '../components/news/newsEpic';
import FileEpic from '../components/files/fileEpic';
import BreadcrumbsEpic from '../components/breadcrumbs/breadcrumbs.epic';
import { combineEpics } from 'redux-observable';

const epics = [
    NewsEpic,
    FileEpic,
    BreadcrumbsEpic
];

const rootEpics = combineEpics(...Object.values(epics));

export default rootEpics;