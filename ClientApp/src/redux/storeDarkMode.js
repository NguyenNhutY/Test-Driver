import { createStore } from "redux";
import reducer from './reducerDarkMode';

const initialState = {
    darkmode: false,
};

const reducer = (state = initialState, action) => {
    switch (action.type) {
        case "TOGGLE_DARKMODE":
            return { ...state, darkmode: !state.darkmode };
        default:
            return state;
    }
};

const store = createStore(reducer);

export default store;
