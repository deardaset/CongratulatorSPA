import { useEffect, useState } from "react";
import { getPeople } from "../api/personApi";

export function usePeople({ upcoming = false, defaultSortBy = 'name' } = {}) {
    const [people, setPeople] = useState([]);
    const [page, setPage] = useState(1);
    const [pageSize] = useState(10);
    const [totalCount, setTotalCount] = useState(0);
    const [sortBy, setSortBy] = useState(defaultSortBy);
    const [searchBy, setSearch] = useState('');
    const [loading, setLoading] = useState(false);

    const fetchPeople = () => {
        setLoading(true);
        getPeople({page, pageSize, sortBy, searchBy, upcoming})
            .then(data => {
                setPeople(data.data);
                setTotalCount(data.totalCount);
            })
        .catch(err => console.error(err))
        .finally(() => setLoading(false));
    };

    useEffect(() => {
        fetchPeople();
    }, [page, sortBy, searchBy])

    const totalPages = Math.ceil(totalCount / pageSize)

    return {
        people,
        page, setPage,
        sortBy, setSortBy,
        searchBy, setSearch,
        loading,
        totalPages,
        reload: fetchPeople,
    };
}