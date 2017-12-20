import { h, Component } from 'preact';
import { Link } from 'react-router-dom';
import * as FetchNewsActions from './newsActions';

// import { Container, Image, Table, Button } from 'semantic-ui-react';

class NewsList extends Component {

    componentWillMount() {
        this.props.actions.getNews({page: 1});
    }

    renderTableBody(items) {
        return (
            items.map((news) => {
                return (
                // <Table.Row key={news.id}>
                //     <Table.Cell>
                //         {news.title}
                //     </Table.Cell>
                //     <Table.Cell>
                //         <Link to={`/news/${news.id}`}>
                //             <Image avatar src={require('../../../assets/images/image.png')} />
                //         </Link>
                //     </Table.Cell>
                //     <Table.Cell>
                //         {/* <Button onClick={() => askForDeleting(news.id)}>Delete</Button> */}
                //         <Button>Delete</Button>
                //     </Table.Cell>
                // </Table.Row>
                <div>news body</div>
                )
            })
        );
    }

    render() {
        const { items } = this.props.news;
        const { gotoCreateNews } = this.props.actions;
        return (
            // <Container >
            //     <Button style={{ marginTop: '5em' }} onClick={gotoCreateNews}><Link to='/news/create'>Create</Link></Button>
            //     <Table color={'red'} key={'red'}  >
            //         <Table.Header>
            //             <Table.Row>
            //                 <Table.HeaderCell>News</Table.HeaderCell>
            //                 <Table.HeaderCell>Preview</Table.HeaderCell>
            //                 <Table.HeaderCell></Table.HeaderCell>
            //             </Table.Row>
            //         </Table.Header>
            //         <Table.Body>
            //             {this.renderTableBody(items)}
            //         </Table.Body>
            //     </Table>
            // </Container>
            <div>news</div>
        );
    }
}

export default NewsList;
