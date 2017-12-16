import * as React from 'react';
import { render } from 'react-dom';
import { Link, Redirect } from 'react-router-dom';
import { RouteComponentProps } from 'react-router';
import * as FetchNewsActions from './newsActions';
import { NewsModel, NewsItem, NewsListState, NewsFilter } from './models';

import { Container, Image, Table, Button } from 'semantic-ui-react';

export interface Props extends RouteComponentProps<void> {
    news: NewsModel;
    actions: typeof FetchNewsActions;
}

class NewsList extends React.Component<Props, {}> {

    componentWillMount() {
        let filter = new NewsFilter();
        filter.page = 1;
        this.props.actions.getNews(filter);
    }

    renderTableBody(items: NewsItem[]) {
        return (
            items.map((news) => {
                return (<Table.Row key={news.id}>
                    <Table.Cell>
                        {news.title}
                    </Table.Cell>
                    <Table.Cell>
                        <Link to={`/news/${news.id}`}>
                            <Image avatar src={require('../../../assets/images/image.png')} />
                        </Link>
                    </Table.Cell>
                    <Table.Cell>
                        {/* <Button onClick={() => askForDeleting(news.id)}>Delete</Button> */}
                        <Button>Delete</Button>
                    </Table.Cell>
                </Table.Row>)
            })
        );
    }

    render() {
        const { items } = this.props.news;
        const { gotoCreateNews } = this.props.actions;
        return (
            <Container >
                <Button style={{ marginTop: '5em' }} onClick={gotoCreateNews}><Link to='/news/create'>Create</Link></Button>
                <Table color={'red'} key={'red'}  >
                    <Table.Header>
                        <Table.Row>
                            <Table.HeaderCell>News</Table.HeaderCell>
                            <Table.HeaderCell>Preview</Table.HeaderCell>
                            <Table.HeaderCell></Table.HeaderCell>
                        </Table.Row>
                    </Table.Header>
                    <Table.Body>
                        {this.renderTableBody(items)}
                    </Table.Body>
                </Table>
            </Container>
        );
    }
}

export default NewsList;
