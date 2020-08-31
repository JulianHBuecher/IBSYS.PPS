import React from "react";
import { Table } from "reactstrap";

export default function ProductionPlan() {
  return (
    <form>
      <Table responsive borderless>
        <thead>
          <tr>
            <th>Produkt</th>
            <th></th>
            <th>Periode</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td></td>
            <td>1</td>
            <td>2</td>
            <td>3</td>
          </tr>
          <tr>
            <td>Kinderfahrrad</td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
          </tr>
          <tr>
            <td>Damenfahrrad</td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
          </tr>
          <tr>
            <td>Herrenfahrrad</td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
          </tr>
        </tbody>
      </Table>
    </form>
  );
}
