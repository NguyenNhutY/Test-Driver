
import { OnTapLT } from "./pages/OnTapLT";
import { ThiLT } from "./pages/ThiLT";
import { Home } from "./pages/Home";
import { PageNotFound } from "./pages/PageNotFound";
import { About } from "./pages/About"

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/hoc-ly-thuyet',
    element: <OnTapLT />
  },
  {
    path: '/thi-ly-thuyet',
    element: <ThiLT />
    },
    {
        path: '/hoc-ly-thuyet*',
        element: <PageNotFound />
    },
    {
        path: '/thi-ly-thuyet*',
        element: <PageNotFound />
    },
    {
        path: '/*',
        element: <PageNotFound />
    },
    {
        path: '/about',
        element: <About />
    },
    {
        path: '/about*',
        element: <About />
    },
];

export default AppRoutes;
