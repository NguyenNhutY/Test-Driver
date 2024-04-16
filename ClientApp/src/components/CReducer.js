
import React from 'react';
import { connect } from 'react-redux';
import { increment, decrement } from '../redux/actions';

const ExampleComponent = ({ count, increment, decrement }) => {
    return (
        <div>
            <h2>Count: {count}</h2>
            <button onClick={increment}>Increment</button>
            <button onClick={decrement}>Decrement</button>
        </div>
    );
};

const mapStateToProps = (state) => ({
    count: state.counter.count
});

export default connect(mapStateToProps, { increment, decrement })(ExampleComponent);
