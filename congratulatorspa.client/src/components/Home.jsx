import { useState, useEffect } from 'react';
import { upcomingPeople } from "../api/personApi";

function calculateDaysToBirthday(birthDate) {
  var result = '';
  const today = new Date();
  const normalizedToday = new Date(
    today.getFullYear(),
    today.getMonth(),
    today.getDate()
  );
  const newBirthDate = new Date(birthDate);
  const nextBirthday = new Date(normalizedToday.getFullYear(), newBirthDate.getMonth(), newBirthDate.getDate());

  const MS_IN_DAY = 1000 * 60 * 60 * 24;
  const diffDays = Math.round((nextBirthday - normalizedToday) / MS_IN_DAY);

  if (diffDays === 0) {
    result = 'Today';
  } else if (diffDays === 1) {
    result = 'Tomorrow';
  } else {
    result = `${diffDays} days`
  }

  return result;
};

const Home = () => {
  const [people, setPeople] = useState([]);
  const [sortBy, setSortBy] = useState('birthdate');
  const [searchBy, setSearch] = useState('');
  //Pagination
  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);

   useEffect(() => {
    upcomingPeople({page, pageSize, sortBy, searchBy})
      .then(data => {
        setPeople(data.data);
        setTotalCount(data.totalCount);
      })
      .catch(err => console.error(err));
  }, [page, sortBy, searchBy]);

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <main className='container'>
      <h1>Todays and upcoming birthdays</h1>      
      <div className='action-panel'>
        <label>
            Sort by:{' '}
            <select value={sortBy} onChange={e => setSortBy(e.target.value)}>
                <option value="name">Name</option>
                <option value="birthdate">Birthdate</option>
                <option value="age">Age</option>
                <option value="relationship">Relationship</option>
            </select>
        </label> 
        <label>
            Search:{' '}
            <input 
              type="text" 
              placeholder="Search" 
              value={searchBy} 
              onChange={e => setSearch(e.target.value)} 
            /> 
        </label>
      </div>     
        <table>
          <thead>
            <tr>
              <th>Photo</th>
              <th>Name</th>
              <th>Birthdate</th>
              <th>Age</th>
              <th>To Birthday</th>
              <th>Relationship</th>
            </tr>
          </thead>
          <tbody>
            {people.map(p => (
              <tr key={p.guid}>
                <td className='avatar'>
                  <img src={p.photoUrl} alt="user-photo"/>
                </td>
                <td>{p.name}</td>
                <td>{new Date(p.birthDate).toLocaleDateString('ru-RU')}</td>
                <td>{p.age}</td>
                <td>{calculateDaysToBirthday(p.birthDate)}</td>
                <td>{p.relationshipType}</td>
              </tr>
            ))}
          </tbody>
        </table>
      <div className="pagination">
        <button
          className="icon-button"
          disabled={page === 1}
          onClick={() => setPage(p => p - 1)}
        >
          <img src="/left-arrow.png" alt="Previous page" />
        </button>

        <span>{page} / {totalPages}</span>

        <button
          className="icon-button"
          disabled={page === totalPages}
          onClick={() => setPage(p => p + 1)}
        >
          <img src="/right-arrow.png" alt="Next page" />
        </button>
      </div>
    </main>
  )
}

export default Home