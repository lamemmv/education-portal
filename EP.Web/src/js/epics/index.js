import NewsEpic from '../components/news/newsEpic';
import FileEpic from '../components/files/fileEpic';
import BreadcrumbsEpic from '../components/breadcrumbs/epic';
import FolderEpic from '../components/folders/epic';
import DialogEpic from '../components/dialogs/epic';
import { combineEpics } from 'redux-observable';

const epics = [
    NewsEpic,
    FileEpic,
    FolderEpic,
    BreadcrumbsEpic,
    DialogEpic
];

const rootEpics = combineEpics(...Object.values(epics));

export default rootEpics;