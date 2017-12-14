export interface NewsListState {
    listState: NewsModel;
    getNews: (page: number) => NewsItem[];
    askForDeleting: (id: string) => any;
}

export interface NewsCreateState {
    id: string;
    loading: boolean,
    error: string;
}

export interface NewsStoreState {
    createState: NewsCreateState;
    listState: NewsModel
}

export interface NewsModel {
    items: NewsItem[];
    page: number;
    loading: boolean,
    error: string;
}

export interface NewsItem {
    id: string;
    title: string;
    ingress: string;
    content: string;
    published: boolean;
    publishedDate?: Date;
}

export class NewsFilter {
    page: number;
    extensions: string[]
}

export interface NewsFetchSuccess {
    totalPages: number;
    totalItems: number;
    items: NewsItem[];
    page: number;
    size: number;
}

export interface NewsFetchFail {
    message: string;
    page: number;
}
