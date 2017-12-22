import { h, Component } from 'preact';
import { Link } from 'react-router-dom';
import * as FetchNewsActions from './newsActions';

// import { Container, Image, Table, Button } from 'semantic-ui-react';

class NewsList extends Component {

    componentWillMount() {
        this.props.actions.getNews({ page: 1 });
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
            // <table class="bordered">
            //     <thead>
            //         <tr>
            //             <th>ID</th>
            //             <th>Name</th>
            //             <th>Link</th>
            //             <th>Status</th>
            //         </tr>
            //     </thead>
            //     <tbody>
            //         <tr>
            //             <td data-title="ID">1</td>
            //             <td data-title="Name">Material Design Color Palette</td>
            //             <td data-title="Link">
            //                 <a href="https://github.com/zavoloklom/material-design-color-palette" target="_blank">GitHub</a>
            //             </td>
            //             <td data-title="Status">Completed</td>
            //         </tr>
            //         <tr>
            //             <td data-title="ID">2</td>
            //             <td data-title="Name">Material Design Iconic Font</td>
            //             <td data-title="Link">
            //                 <a href="https://codepen.io/zavoloklom/pen/uqCsB" target="_blank">Codepen</a>
            //                 <a href="https://github.com/zavoloklom/material-design-iconic-font" target="_blank">GitHub</a>
            //             </td>
            //             <td data-title="Status">Completed</td>
            //         </tr>
            //     </tbody>
            // </table>

            <ul class="mdc-list mdc-list--dense">
                <li class="mdc-list-item">
                    <span class="mdc-list-item__start-detail" role="presentation">
                        <i class="material-icons" aria-hidden="true">folder</i>
                    </span>
                    <span class="mdc-list-item__text">
                        <span class='mdc-list-item__end-detail'>column 1</span>                        
                    </span>
                    <span class="mdc-list-item__end-detail">
                        <time datetime="2014-01-28T04:36:00.000Z">4:36pm</time>
                        <i class="material-icons" arial-label="Unread message">chat_bubble</i>
                    </span>
                </li>
            </ul>
        );
    }
}

export default NewsList;
