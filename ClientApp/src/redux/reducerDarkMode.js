const initialState = {
    darkmode: false,
};

const reducer = (state = initialState, action) => {
    switch (action.type) {
        case 'TOGGLE_DARKMODE':
            return { ...state, darkmode: !state.darkmode };
        default:
            return state;
    }
};

export default reducer;
